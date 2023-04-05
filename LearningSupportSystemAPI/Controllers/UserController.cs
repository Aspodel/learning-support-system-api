using AutoMapper;
using LearningSupportSystemAPI.Contract;
using LearningSupportSystemAPI.Core.Entities;
using LearningSupportSystemAPI.DataObjects;
using LearningSupportSystemAPI.Repository;
using LearningSupportSystemAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text;

namespace LearningSupportSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        #region [Fields]
        private readonly UserManager _userManager;
        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger;
        private readonly IGenerateIdService _generateIdService;
        #endregion

        #region [Ctor]
        public UserController(UserManager userManager, IMapper mapper, ILogger<UserController> logger, IGenerateIdService generateIdService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
            _generateIdService = generateIdService;
        }
        #endregion

        #region [GET]
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            var users = await _userManager.FindAll().ToListAsync(cancellationToken);
            return Ok(_mapper.Map<IEnumerable<UserDTO>>(users));
        }

        [HttpGet("{idCard}")]
        public async Task<IActionResult> Get(string idCard)
        {
            var user = await _userManager.FindByIdCardAsync(idCard);
            if (user is null)
                return NotFound();

            return Ok(_mapper.Map<UserDTO>(user));
        }
        #endregion

        #region [POST]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDTO dto)
        {
            var user = _mapper.Map<User>(dto);
            user.IdCard = _generateIdService.GenerateUserId();

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                _logger.LogError("Unable to create user {username}. Result details: {result}", dto.Username, string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                return BadRequest(result);
            }

            // Send email for account confirmation
            //await SendEmailConfirmation(user);

            // Add user to specified roles
            var addToRolesResult = await _userManager.AddToRolesAsync(user, dto.Roles);
            if (!addToRolesResult.Succeeded)
            {
                _logger.LogError("Unable to assign user {username} to roles {roles}. Result details: {result}", dto.Username, string.Join(", ", dto.Roles), string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                //return BadRequest("Fail to add role");
            }

            return Ok(_mapper.Map<UserDTO>(user));
        }
        #endregion

        #region [PUT]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UserDTO dto)
        {
            var user = await _userManager.FindByIdCardAsync(dto.IdCard);
            if (user is null || user.IsDeleted)
                return NotFound();

            _mapper.Map(dto, user);
            await _userManager.UpdateAsync(user);

            ICollection<string> requestRoles = dto.Roles;
            ICollection<string> originalRoles = await _userManager.GetRolesAsync(user);

            // Delete Roles
            ICollection<string> deleteRoles = originalRoles.Except(requestRoles).ToList();
            if (deleteRoles.Count > 0)
                await _userManager.RemoveFromRolesAsync(user, deleteRoles);

            // Add Roles
            ICollection<string> newRoles = requestRoles.Except(originalRoles).ToList();
            if (newRoles.Count > 0)
                await _userManager.AddToRolesAsync(user, newRoles);

            return NoContent();
        }
        #endregion

        #region [DELETE]
        [HttpDelete("{idCard}")]
        public async Task<IActionResult> Delete(string idCard)
        {
            var user = await _userManager.FindByIdCardAsync(idCard);
            if (user is null)
                return NotFound();

            user.IsDeleted = true;
            await _userManager.UpdateAsync(user);

            return NoContent();
        }
        #endregion
    }
}
