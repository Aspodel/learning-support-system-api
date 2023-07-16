using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearningSupportSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        #region [Fields]
        private readonly ICourseRepository _courseRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;
        private readonly IGenerateIdService _generateIdService;
        #endregion

        #region [Ctor]
        public CourseController(ICourseRepository courseRepository, IDepartmentRepository departmentRepository, IMapper mapper, IGenerateIdService generateIdService)
        {
            _courseRepository = courseRepository;
            _departmentRepository = departmentRepository;
            _mapper = mapper;
            _generateIdService = generateIdService;
        }
        #endregion

        #region [GET]
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            var courses = await _courseRepository.FindAll().ToListAsync(cancellationToken);
            return Ok(_mapper.Map<IEnumerable<CourseDTO>>(courses));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var course = await _courseRepository.FindByIdAsync(id);
            if (course is null)
                return NotFound();

            return Ok(_mapper.Map<CourseDTO>(course));
        }
        [HttpGet("department/{departmentId}")]
        public async Task<IActionResult> GetByDepartment(int departmentId, CancellationToken cancellationToken = default)
        {
            var department = await _departmentRepository.FindByIdAsync(departmentId, cancellationToken);
            if (department is null)
                return BadRequest("Department is not exist");

            var courses = await _courseRepository.FindAllByDepartment(department.Id).ToListAsync(cancellationToken);
            return Ok(_mapper.Map<IEnumerable<CourseDTO>>(courses));
        }
        #endregion

        #region [POST]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCourseDTO dto, CancellationToken cancellationToken = default)
        {
            var department = await _departmentRepository.FindByIdAsync(dto.DepartmentId, cancellationToken);
            if (department is null)
                return BadRequest("Department is not exist");

            var course = _mapper.Map<Course>(dto);
            course.Department = department;
            course.CourseCode = _generateIdService.GenerateCourseCode(course);

            _courseRepository.Add(course);
            await _courseRepository.SaveChangesAsync(cancellationToken);

            return Ok(_mapper.Map<CourseDTO>(course));
        }

        [HttpPost("create-from-excel")]
        public async Task<IActionResult> CreateFromExcel(IFormFile file, CancellationToken cancellationToken = default)
        {
            var datatable = await file.GetExcelDataTable(cancellationToken);
            var courses = datatable.GetEntitiesFromDataTable<Course>();

            // Get distinct department ids from the courses
            List<int> departmentIds = courses.Select(c => c.DepartmentId).Distinct().ToList();

            // Retrieve departments from the database based on the department ids
            var departmentsFromDb = await _departmentRepository.FindAll()
                .Where(d => departmentIds.Contains(d.Id))
                .ToListAsync(cancellationToken);

            // Check if all department ids exist in the database
            var missingDepartmentIds = departmentIds.Except(departmentsFromDb.Select(d => d.Id));
            if (missingDepartmentIds.Any())
            {
                return BadRequest($"Departments {string.Join(", ", missingDepartmentIds)} do not exist");
            }

            // Assign departments and generate course codes
            foreach (var course in courses)
            {
                course.Department = departmentsFromDb.FirstOrDefault(d => d.Id == course.DepartmentId);
                course.CourseCode = _generateIdService.GenerateCourseCode(course);
                _courseRepository.Add(course);
                await _courseRepository.SaveChangesAsync(cancellationToken);
            }

            return Ok(_mapper.Map<IEnumerable<CourseDTO>>(courses));
        }

        [HttpPost("create-schedule-recommendation")]
        public async Task<IActionResult> CreateScheduleRecommendation([FromBody] ScheduleRecommendationDTO dto, CancellationToken cancellationToken = default)
        {
            List<Course> courseList = new List<Course>();
            foreach (var courseId in dto.Courses)
            {
                var course = await _courseRepository.FindByIdAsync(courseId, cancellationToken);
                if (course is null)
                    return BadRequest($"Course {courseId} is not exist");

                courseList.Add(course);
            }

            var schedules = courseList.RecommendSchedules(dto.Constraint);
            if (schedules is null)
                return Ok("No appropriate schedules");

            var result = new List<List<ClassDTO>>();
            foreach (var schedule in schedules)
            {
                result.Add(_mapper.Map<List<ClassDTO>>(schedule.ClassTimes));
            }
            return Ok(result);
        }
        #endregion

        #region [PUT]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CourseDTO dto, CancellationToken cancellationToken = default)
        {
            var course = await _courseRepository.FindByIdAsync(dto.Id, cancellationToken);
            if (course is null)
                return NotFound();

            _mapper.Map(dto, course);
            _courseRepository.Update(course);
            await _courseRepository.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
        #endregion

        #region [DELETE]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
        {
            var course = await _courseRepository.FindByIdAsync(id, cancellationToken);
            if (course is null)
                return NotFound();

            _courseRepository.Delete(course);
            await _courseRepository.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
        #endregion
    }
}
