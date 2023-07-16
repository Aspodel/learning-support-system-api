using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearningSupportSystemAPI;

[Route("api/[controller]")]
[ApiController]
public class ClassController : ControllerBase
{
    #region [Fields]
    private readonly IClassRepository _classRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly IGradeColumnRepository _gradeColumnRepository;
    // private readonly IRoomRepository _roomRepository;
    // private readonly ISemesterRepository _semesterRepository;
    private readonly LecturerManager _lecturerManager;
    private readonly IMapper _mapper;
    private readonly IGenerateIdService _generateIdService;
    #endregion

    #region [Ctor]
    public ClassController(IClassRepository classRepository, ICourseRepository courseRepository, IGradeColumnRepository gradeColumnRepository, LecturerManager lecturerManager, IMapper mapper, IGenerateIdService generateIdService)
    {
        _classRepository = classRepository;
        _courseRepository = courseRepository;
        _gradeColumnRepository = gradeColumnRepository;
        // _roomRepository = roomRepository;
        // _semesterRepository = semesterRepository;
        _lecturerManager = lecturerManager;
        _mapper = mapper;
        _generateIdService = generateIdService;
    }
    #endregion

    #region [GET]
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
    {
        var classes = await _classRepository.FindAll().ToListAsync(cancellationToken);
        return Ok(_mapper.Map<IEnumerable<ClassDTO>>(classes));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var cla = await _classRepository.FindByIdAsync(id);
        if (cla is null)
            return NotFound();

        cla.GradeColumns = cla.GradeColumns!.OrderBy(gc => gc.Order).ToList();
        return Ok(_mapper.Map<ClassDTO>(cla));
    }
    #endregion

    #region [POST]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateClassDTO dto, CancellationToken cancellationToken = default)
    {
        var course = await _courseRepository.FindByIdAsync(dto.CourseId, cancellationToken);
        if (course is null)
            return NotFound("Course not found");

        var lecturer = await _lecturerManager.FindByIdAsync(dto.LecturerId);
        if (lecturer is null)
            return NotFound("Lecturer not found");

        var cla = _mapper.Map<Class>(dto);
        cla.Course = course;
        cla.Lecturer = lecturer;
        cla.ClassCode = _generateIdService.GenerateClassCode(cla);

        _classRepository.Add(cla);
        await _classRepository.SaveChangesAsync(cancellationToken);

        return Ok(_mapper.Map<ClassDTO>(cla));
    }

    [HttpPost("create-from-excel")]
    public async Task<IActionResult> CreateFromExcel(IFormFile file, CancellationToken cancellationToken = default)
    {
        var datatable = await file.GetExcelDataTable(cancellationToken);
        var dtos = datatable.GetEntitiesFromDataTable<CreateClassExcelDTO>();

        List<string> courseCodes = dtos.Select(c => c.CourseCode).Distinct().ToList();
        List<string> lecturerIds = dtos.Select(c => c.LecturerIdCard).Distinct().ToList();

        var courses = await _courseRepository.FindAll().Where(c => courseCodes.Contains(c.CourseCode)).ToListAsync(cancellationToken);
        var lecturers = await _lecturerManager.FindAll().Where(l => lecturerIds.Contains(l.IdCard)).ToListAsync(cancellationToken);

        var missingCourseIds = courseCodes.Except(courses.Select(c => c.CourseCode)).ToList();
        if (missingCourseIds.Any())
            return NotFound($"Course with id {string.Join(", ", missingCourseIds)} not found");

        var missingLecturerIds = lecturerIds.Except(lecturers.Select(l => l.IdCard)).ToList();
        if (missingLecturerIds.Any())
            return NotFound($"Lecturer with idCard {string.Join(", ", missingLecturerIds)} not found");

        var classes = new List<Class>();
        foreach (var dto in dtos)
        {
            var cl = _mapper.Map<Class>(dto);
            cl.Course = courses.FirstOrDefault(c => c.CourseCode == dto.CourseCode);
            cl.Lecturer = lecturers.FirstOrDefault(l => l.IdCard == dto.LecturerIdCard);
            cl.ClassCode = _generateIdService.GenerateClassCode(cl);
            _classRepository.Add(cl);
            await _classRepository.SaveChangesAsync(cancellationToken);
            classes.Add(cl);
        }

        return Ok(_mapper.Map<IEnumerable<ClassDTO>>(classes));
    }

    #endregion

    #region [PUT]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ClassDTO dto, CancellationToken cancellationToken = default)
    {
        var cla = await _classRepository.FindByIdAsync(dto.Id, cancellationToken);
        if (cla is null)
            return NotFound();

        _mapper.Map(dto, cla);
        
        // ICollection<GradeColumn> gradeColumns = cla.GradeColumns!.ToList();
        // ICollection<int> requestGradeColumns = dto.GradeColumns!.Select(gc => gc.Id).ToList();
        // ICollection<int> originalGradeColumns = cla.GradeColumns!.Select(gc => gc.Id).ToList();

        // ICollection<int> deletedGradeColumns = originalGradeColumns.Except(requestGradeColumns).ToList();
        // if (deletedGradeColumns.Any())
        // {
        //     foreach (var gradeColumnId in deletedGradeColumns)
        //     {
        //         var gradeColumn = gradeColumns.FirstOrDefault(gc => gc.Id == gradeColumnId);
        //         gradeColumns.Remove(gradeColumn);
        //         _gradeColumnRepository.Delete(gradeColumn);
        //     }
        // }

        // ICollection<int> addedGradeColumns = requestGradeColumns.Except(originalGradeColumns).ToList();
        // if (addedGradeColumns.Any())
        // {
        //     foreach (var gradeColumnId in addedGradeColumns)
        //     {
        //         var gradeColumnDTO = dto.GradeColumns!.FirstOrDefault(gc => gc.Id == gradeColumnId);
        //         var gradeColumn = _mapper.Map<GradeColumn>(gradeColumnDTO);
        //         gradeColumns.Add(gradeColumn);
        //     }
        // }

        // for (int i = 0; i < gradeColumns.Count; i++)
        // {
        //     var gradeColumn = gradeColumns.ElementAt(i);
        //     gradeColumn.Order = i;
        // }
        // cla.GradeColumns = gradeColumns;
        _classRepository.Update(cla);
        await _classRepository.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
    #endregion

    #region [DELETE]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
    {
        var cla = await _classRepository.FindByIdAsync(id, cancellationToken);
        if (cla is null)
            return NotFound();

        _classRepository.Delete(cla);
        await _classRepository.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
    #endregion
}
