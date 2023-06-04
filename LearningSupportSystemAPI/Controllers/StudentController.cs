using AutoMapper;
using LearningSupportSystemAPI.Contract;
using LearningSupportSystemAPI.Core.Entities;
using LearningSupportSystemAPI.Core.Entities.JoinTables;
using LearningSupportSystemAPI.DataObjects;
using LearningSupportSystemAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearningSupportSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        #region [Fields]
        private readonly StudentManager _studentManager;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IClassRepository _classRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<StudentController> _logger;
        private readonly IGenerateIdService _generateIdService;
        #endregion

        public StudentController(StudentManager studentManager, IDepartmentRepository departmentRepository, IClassRepository classRepository, IMapper mapper, ILogger<StudentController> logger, IGenerateIdService generateIdService)
        {
            _studentManager = studentManager;
            _departmentRepository = departmentRepository;
            _classRepository = classRepository;
            _mapper = mapper;
            _logger = logger;
            _generateIdService = generateIdService;
        }

        #region [GET]
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            var students = await _studentManager.FindAll().ToListAsync(cancellationToken);
            return Ok(_mapper.Map<IEnumerable<StudentDTO>>(students));
        }

        [HttpGet("{idCard}")]
        public async Task<IActionResult> Get(string idCard)
        {
            var student = await _studentManager.FindByIdCardAsync(idCard);
            if (student is null)
                return NotFound();

            return Ok(_mapper.Map<StudentDTO>(student));
        }
        #endregion

        #region [POST]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStudentDTO dto)
        {
            var department = await _departmentRepository.FindByIdAsync(dto.DepartmentId);
            if (department is null)
                return BadRequest("Department not found");

            var student = _mapper.Map<Student>(dto);
            student.Department = department;
            student.StartYear = dto.StartYear ?? DateTime.UtcNow.Year;
            student.IdCard = _generateIdService.GenerateStudentIdCard(student);
            student.UserName = student.IdCard;


            var result = await _studentManager.CreateAsync(student, dto.DateOfBirth.ToString("ddMMyy"));
            if (!result.Succeeded)
            {
                _logger.LogError("Unable to create user {username}. Result details: {result}", dto.FirstName, string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                return BadRequest(result);
            }

            // Add user to specified roles
            var addToRolesResult = await _studentManager.AddToRolesAsync(student, dto.Roles);
            if (!addToRolesResult.Succeeded)
            {
                _logger.LogError("Unable to assign user {username} to roles {roles}. Result details: {result}", student.IdCard, string.Join(", ", dto.Roles), string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                //return BadRequest("Fail to add role");
            }

            return Ok(_mapper.Map<StudentDTO>(student));
        }
        #endregion

        #region [PUT]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] StudentDTO dto)
        {
            var student = await _studentManager.FindByIdCardAsync(dto.IdCard);
            if (student is null || student.IsDeleted)
                return NotFound();

            _mapper.Map(dto, student);
            await _studentManager.UpdateAsync(student);

            ICollection<string> requestRoles = dto.Roles;
            ICollection<string> originalRoles = await _studentManager.GetRolesAsync(student);

            // Delete Roles
            ICollection<string> deleteRoles = originalRoles.Except(requestRoles).ToList();
            if (deleteRoles.Count > 0)
                await _studentManager.RemoveFromRolesAsync(student, deleteRoles);

            // Add Roles
            ICollection<string> newRoles = requestRoles.Except(originalRoles).ToList();
            if (newRoles.Count > 0)
                await _studentManager.AddToRolesAsync(student, newRoles);

            return NoContent();
        }

        [HttpPut("register-courses")]
        public async Task<IActionResult> RegisterCourses([FromBody] RegisterCoursesDTO dto)
        {
            var student = await _studentManager.FindByIdCardAsync(dto.IdCard);
            if (student is null || student.IsDeleted)
                return NotFound();

            ICollection<StudentClass> classes = student.RegisteredClasses;
            ICollection<int> requestClasses = dto.RegisteredClasses;
            ICollection<int> originalClasses = student.RegisteredClasses.Select(c => c.ClassId).ToList();

            // Delete Classes
            ICollection<int> deleteClasses = originalClasses.Except(requestClasses).ToList();
            if (deleteClasses.Count > 0)
            {
                classes = classes.Where(c => !deleteClasses.Contains(c.ClassId)).ToList();
            }

            // Add Classes
            ICollection<int> newClasses = requestClasses.Except(originalClasses).ToList();
            if (newClasses.Count > 0)
            {
                foreach (var item in newClasses)
                {
                    var cla = await _classRepository.FindByIdAsync(item);
                    if (cla is null)
                        return BadRequest($"ClassId {item} is not valid");

                    classes.Add(new StudentClass
                    {
                        Class = cla,
                        Student = student
                    });
                }
            }

            classes = classes.OrderBy(c => c.Class!.Day).ThenBy(c => c.Class!.StartTime).ToList();
            student.RegisteredClasses = classes;

            await _classRepository.SaveChangesAsync();
            await _studentManager.UpdateAsync(student);

            return NoContent();
        }
        #endregion

        #region [DELETE]
        [HttpDelete("{idCard}")]
        public async Task<IActionResult> Delete(string idCard)
        {
            var user = await _studentManager.FindByIdCardAsync(idCard);
            if (user is null)
                return NotFound();

            user.IsDeleted = true;
            await _studentManager.UpdateAsync(user);

            return NoContent();
        }
        #endregion
    }
}
