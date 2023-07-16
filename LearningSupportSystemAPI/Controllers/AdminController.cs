using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearningSupportSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        #region [Fields]
        private readonly AdminManager _adminManager;
        private readonly IMapper _mapper;
        private readonly ILogger<AdminController> _logger;
        private readonly IGenerateIdService _generateIdService;
        #endregion

        #region [Ctor]
        public AdminController(AdminManager adminManager, IMapper mapper, ILogger<AdminController> logger, IGenerateIdService generateIdService)
        {
            _adminManager = adminManager;
            _mapper = mapper;
            _logger = logger;
            _generateIdService = generateIdService;
        }
        #endregion

        #region [GET]
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            var admins = await _adminManager.FindAll().ToListAsync(cancellationToken);
            return Ok(_mapper.Map<IEnumerable<AdminDTO>>(admins));
        }

        [HttpGet("{idCard}")]
        public async Task<IActionResult> Get(string idCard)
        {
            var admin = await _adminManager.FindByIdCardAsync(idCard);
            if (admin is null)
                return NotFound();

            return Ok(_mapper.Map<AdminDTO>(admin));
        }
        #endregion

        #region [POST]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAdminDTO dto)
        {
            var admin = _mapper.Map<Administrator>(dto);
            admin.IdCard = _generateIdService.GenerateUserIdCard();

            var result = await _adminManager.CreateAsync(admin, dto.Password);
            if (!result.Succeeded)
            {
                _logger.LogError("Unable to create user {username}. Result details: {result}", dto.Username, string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                return BadRequest(result);
            }

            // Send email for account confirmation
            //await SendEmailConfirmation(user);

            // Add user to specified roles
            var addToRolesResult = await _adminManager.AddToRolesAsync(admin, dto.Roles);
            if (!addToRolesResult.Succeeded)
            {
                _logger.LogError("Unable to assign user {username} to roles {roles}. Result details: {result}", dto.Username, string.Join(", ", dto.Roles), string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                //return BadRequest("Fail to add role");
            }

            return Ok(_mapper.Map<AdminDTO>(admin));
        }
        #endregion

        #region [PUT]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] AdminDTO dto)
        {
            var admin = await _adminManager.FindByIdCardAsync(dto.IdCard);
            if (admin is null || admin.IsDeleted)
                return NotFound();

            _mapper.Map(dto, admin);
            await _adminManager.UpdateAsync(admin);

            ICollection<string> requestRoles = dto.Roles;
            ICollection<string> originalRoles = await _adminManager.GetRolesAsync(admin);

            // Delete Roles
            ICollection<string> deleteRoles = originalRoles.Except(requestRoles).ToList();
            if (deleteRoles.Count > 0)
                await _adminManager.RemoveFromRolesAsync(admin, deleteRoles);

            // Add Roles
            ICollection<string> newRoles = requestRoles.Except(originalRoles).ToList();
            if (newRoles.Count > 0)
                await _adminManager.AddToRolesAsync(admin, newRoles);

            return NoContent();
        }
        #endregion

        #region [DELETE]
        [HttpDelete("{idCard}")]
        public async Task<IActionResult> Delete(string idCard)
        {
            var admin = await _adminManager.FindByIdCardAsync(idCard);
            if (admin is null)
                return NotFound();

            admin.IsDeleted = true;
            await _adminManager.UpdateAsync(admin);

            return NoContent();
        }
        #endregion
    }
}
