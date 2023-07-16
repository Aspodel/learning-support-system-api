using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearningSupportSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GradeController : ControllerBase
    {
        #region [Fields]
        private readonly IGradeRepository _gradeRepository;
        private readonly IClassRepository _classRepository;
        private readonly StudentManager _studentManager;
        private readonly IMapper _mapper;
        #endregion

        #region [Ctor]
        public GradeController(IGradeRepository gradeRepository, IClassRepository classRepository, StudentManager studentManager, IMapper mapper)
        {
            _gradeRepository = gradeRepository;
            _classRepository = classRepository;
            _studentManager = studentManager;
            _mapper = mapper;
        }
        #endregion

        #region [GET]
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            var grades = await _gradeRepository.FindAll().ToListAsync(cancellationToken);
            return Ok(_mapper.Map<IEnumerable<GradeDTO>>(grades));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var grade = await _gradeRepository.FindByIdAsync(id);
            if (grade is null)
                return NotFound();

            return Ok(_mapper.Map<GradeDTO>(grade));
        }

        [HttpGet("class/{classId}")]
        public async Task<IActionResult> GetByClass(int classId, CancellationToken cancellationToken = default)
        {
            var cla = await _classRepository.FindByIdAsync(classId, cancellationToken);
            if (cla is null)
                return NotFound();

            var gradeTable = await GenerateGradeTable(cla);
            return Ok(gradeTable);
        }

        [HttpGet("student/{studentId}/column/{columnId}")]
        public async Task<IActionResult> GetByStudentAndGradeColumn(string studentId, int columnId, CancellationToken cancellationToken = default)
        {
            var grade = await _gradeRepository.FindByDetail(studentId, columnId);
            // if (grade is null)
                // return NotFound();

            return Ok(_mapper.Map<GradeDTO>(grade));
        }

        [HttpGet("export/class/{classId}")]
        public async Task<IActionResult> ExportExcelByClass(int classId, CancellationToken cancellationToken = default)
        {
            var cla = await _classRepository.FindByIdAsync(classId, cancellationToken);
            if (cla is null)
                return NotFound();

            var gradeTable = await GenerateGradeTable(cla);
            var fileContentResult = gradeTable.ExportToFile($"{cla.ClassCode} - grade table");
            return fileContentResult;
        }

        private async Task<List<GradeRowDTO>> GenerateGradeTable(Class cla)
        {
            cla.GradeColumns = cla.GradeColumns!.OrderBy(gc => gc.Order).ToList();
            cla.Students = cla.Students!.OrderBy(s => s.Student!.IdCard).ToList();
            List<GradeRowDTO> gradeTable = new List<GradeRowDTO>();

            foreach (var student in cla.Students)
            {
                var gradeRow = new GradeRowDTO();

                gradeRow.Id = student.Student?.IdCard;
                gradeRow.Student = student.Student?.FullName;
                gradeRow.Grades = new Dictionary<string, int?>();

                foreach (var gradeColumn in cla.GradeColumns)
                {
                    var grade = await _gradeRepository.FindByDetail(student.StudentId, gradeColumn.Id);
                    gradeRow.Grades[gradeColumn.Name] = grade?.Value ?? null;
                }

                gradeTable.Add(gradeRow);
            }

            return gradeTable;
        }

        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetByStudent(string studentId, CancellationToken cancellationToken = default)
        {
            var grades = await _gradeRepository.FindByStudent(studentId, cancellationToken);

            var result = grades
                .GroupBy(g => g.GradeColumn.Class)
                .Select(group => new
                {
                    Course = group.Key.Course.Name,
                    Grades = group.OrderBy(g => g.GradeColumn.Order)
                                .ToDictionary(
                                    g => g.GradeColumn.Name,
                                    g => new
                                    {
                                        Percentage = g.GradeColumn.Percentage,
                                        Value = g.Value.HasValue && g.GradeColumn.IsPublished == true ? g.Value.Value : null as int?
                                    })
                })
                .ToList();

            return Ok(result);
        }
        #endregion

        #region [POST]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] GradeDTO dto, CancellationToken cancellationToken = default)
        {
            var grade = _mapper.Map<Grade>(dto);
            _gradeRepository.Add(grade);
            await _gradeRepository.SaveChangesAsync(cancellationToken);

            return Ok(_mapper.Map<GradeDTO>(grade));
        }

        [HttpPost("create-from-excel/{classId}")]
        public async Task<IActionResult> CreateFromExcel(int classId, IFormFile file, CancellationToken cancellationToken = default)
        {
            var gradeDataTable = await file.GetExcelDataTable(cancellationToken);
            var gradeTable = gradeDataTable.GetEntitiesFromDataTable<GradeRowDTO>();
            var cla = await _classRepository.FindByIdAsync(classId, cancellationToken);
            if (cla is null)
                return NotFound();

            foreach (var gradeRow in gradeTable)
            {
                var student = cla.Students.FirstOrDefault(s => s.Student!.IdCard == gradeRow.Id);
                if (student is null)
                    continue;

                foreach (var gradeColumn in cla.GradeColumns)
                {
                    var grade = await _gradeRepository.FindByDetail(student.StudentId, gradeColumn.Id);
                    if (grade is null)
                    {
                        grade = new Grade();
                        grade.StudentId = student.StudentId;
                        grade.GradeColumnId = gradeColumn.Id;
                        if (gradeRow.Grades != null && gradeRow.Grades.TryGetValue(gradeColumn.Name, out var gradeValue))
                            grade.Value = (int?)gradeValue;
                        else
                            grade.Value = null;

                        _gradeRepository.Add(grade);
                    }
                    else
                    {
                        if (gradeRow.Grades != null && gradeRow.Grades.TryGetValue(gradeColumn.Name, out var gradeValue))
                            grade.Value = (int?)gradeValue;
                        else
                            grade.Value = null;

                        _gradeRepository.Update(grade);
                    }
                }
            }

            await _gradeRepository.SaveChangesAsync(cancellationToken);

            return Ok();
        }
        #endregion

        #region [PUT]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] GradeDTO dto, CancellationToken cancellationToken = default)
        {
            var grade = await _gradeRepository.FindByIdAsync(dto.Id, cancellationToken);
            if (grade is null)
                return NotFound();

            _mapper.Map(dto, grade);
            _gradeRepository.Update(grade);
            await _gradeRepository.SaveChangesAsync(cancellationToken);

            return NoContent();
        }

        [HttpPut("update-range")]
        public async Task<IActionResult> UpdateRange([FromBody] IEnumerable<GradeDTO> dtos, CancellationToken cancellationToken = default)
        {
            var grades = await _gradeRepository.FindAll().ToListAsync(cancellationToken);
            if (grades is null)
                return NotFound();

            _mapper.Map(dtos, grades);
            _gradeRepository.UpdateRange(grades);
            await _gradeRepository.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
        #endregion

        #region [DELETE]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
        {
            var grade = await _gradeRepository.FindByIdAsync(id, cancellationToken);
            if (grade is null)
                return NotFound();

            _gradeRepository.Delete(grade);
            await _gradeRepository.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
        #endregion
    }
}
