using AutoMapper;
using LearningSupportSystemAPI.Contract;
using LearningSupportSystemAPI.Core.Entities;
using LearningSupportSystemAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearningSupportSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LecturerController : ControllerBase
    {
        #region [Fields]
        private readonly LecturerManager _lecturerManager;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<LecturerController> _logger;
        private readonly IGenerateIdService _generateIdService;
        #endregion

        #region [Ctor]
        public LecturerController(LecturerManager lecturerManager, IDepartmentRepository departmentRepository, IMapper mapper, ILogger<LecturerController> logger, IGenerateIdService generateIdService)
        {
            _lecturerManager = lecturerManager;
            _departmentRepository = departmentRepository;
            _mapper = mapper;
            _logger = logger;
            _generateIdService = generateIdService;
        }
        #endregion

        #region [GET]
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            var lecturers = await _lecturerManager.FindAll().ToListAsync(cancellationToken);
            return Ok(_mapper.Map<IEnumerable<LecturerDTO>>(lecturers));
        }

        [HttpGet("{idCard}")]
        public async Task<IActionResult> Get(string idCard)
        {
            var lecturer = await _lecturerManager.FindByIdCardAsync(idCard);
            if (lecturer is null)
                return NotFound();

            return Ok(_mapper.Map<LecturerDTO>(lecturer));
        }
        #endregion

        #region [POST]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateLecturerDTO dto)
        {
            var department = await _departmentRepository.FindByIdAsync(dto.DepartmentId);
            if (department is null)
                return BadRequest("Department not found");

            var lecturer = _mapper.Map<Lecturer>(dto);
            lecturer.Department = department;
            lecturer.IdCard = _generateIdService.GenerateLecturerIdCard(lecturer);
            lecturer.UserName = lecturer.IdCard;

            var result = await _lecturerManager.CreateAsync(lecturer, dto.DateOfBirth.ToString("ddMMyy"));
            if (!result.Succeeded)
            {
                _logger.LogError("Unable to create user {username}. Result details: {result}", dto.FirstName, string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                return BadRequest(result);
            }

            // Add user to specified roles
            var addToRolesResult = await _lecturerManager.AddToRolesAsync(lecturer, dto.Roles);
            if (!addToRolesResult.Succeeded)
            {
                _logger.LogError("Unable to assign user {username} to roles {roles}. Result details: {result}", lecturer.IdCard, string.Join(", ", dto.Roles), string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                //return BadRequest("Fail to add role");
            }

            return Ok(_mapper.Map<LecturerDTO>(lecturer));
        }
        #endregion

        #region [PUT]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] LecturerDTO dto)
        {
            var lecturer = await _lecturerManager.FindByIdCardAsync(dto.IdCard);
            if (lecturer is null || lecturer.IsDeleted)
                return NotFound();

            _mapper.Map(dto, lecturer);
            await _lecturerManager.UpdateAsync(lecturer);

            ICollection<string> requestRoles = dto.Roles;
            ICollection<string> originalRoles = await _lecturerManager.GetRolesAsync(lecturer);

            // Delete Roles
            ICollection<string> deleteRoles = originalRoles.Except(requestRoles).ToList();
            if (deleteRoles.Count > 0)
                await _lecturerManager.RemoveFromRolesAsync(lecturer, deleteRoles);

            // Add Roles
            ICollection<string> newRoles = requestRoles.Except(originalRoles).ToList();
            if (newRoles.Count > 0)
                await _lecturerManager.AddToRolesAsync(lecturer, newRoles);

            return NoContent();
        }
        #endregion

        #region [DELETE]
        [HttpDelete("{idCard}")]
        public async Task<IActionResult> Delete(string idCard)
        {
            var lecturer = await _lecturerManager.FindByIdCardAsync(idCard);
            if (lecturer is null)
                return NotFound();

            lecturer.IsDeleted = true;
            await _lecturerManager.UpdateAsync(lecturer);

            return NoContent();
        }
        #endregion
    }
}
