using AutoMapper;
using LearningSupportSystemAPI.Contract;
using LearningSupportSystemAPI.Core.Entities;
using LearningSupportSystemAPI.DataObjects;
using LearningSupportSystemAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearningSupportSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        #region [Fields]
        private readonly IClassRepository _classRepository;
        private readonly ICourseRepository _courseRepository;
        // private readonly IRoomRepository _roomRepository;
        // private readonly ISemesterRepository _semesterRepository;
        private readonly LecturerManager _lecturerManager;
        private readonly IMapper _mapper;
        private readonly IGenerateIdService _generateIdService;
        #endregion

        #region [Ctor]
        public ClassController(IClassRepository classRepository, ICourseRepository courseRepository, LecturerManager lecturerManager, IMapper mapper, IGenerateIdService generateIdService)
        {
            _classRepository = classRepository;
            _courseRepository = courseRepository;
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
            var classes = datatable.GetEntitiesFromDataTable<Class>();

            List<int> courseIds = classes.Select(c => c.CourseId).Distinct().ToList();
            List<string> lecturerIds = classes.Select(c => c.LecturerId).Distinct().ToList();

            var courses = await _courseRepository.FindAll().Where(c => courseIds.Contains(c.Id)).ToListAsync(cancellationToken);
            var lecturers = await _lecturerManager.FindAll().Where(l => lecturerIds.Contains(l.Id)).ToListAsync(cancellationToken);

            var missingCourseIds = courseIds.Except(courses.Select(c => c.Id)).ToList();
            if (missingCourseIds.Any())
                return NotFound($"Course with id {string.Join(", ", missingCourseIds)} not found");

            var missingLecturerIds = lecturerIds.Except(lecturers.Select(l => l.Id)).ToList();
            if (missingLecturerIds.Any())
                return NotFound($"Lecturer with id {string.Join(", ", missingLecturerIds)} not found");

            foreach (var cla in classes)
            {
                cla.Course = courses.FirstOrDefault(c => c.Id == cla.CourseId);
                cla.Lecturer = lecturers.FirstOrDefault(l => l.Id == cla.LecturerId);
                cla.ClassCode = _generateIdService.GenerateClassCode(cla);
                _classRepository.Add(cla);
                await _classRepository.SaveChangesAsync(cancellationToken);
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
}
