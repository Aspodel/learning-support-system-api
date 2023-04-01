using AutoMapper;
using LearningSupportSystemAPI.Contract;
using LearningSupportSystemAPI.Core.Entities;
using LearningSupportSystemAPI.DataObjects;
using LearningSupportSystemAPI.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearningSupportSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        #region [Fields]
        private readonly UserManager _userManager;
        private readonly IMapper _mapper;
        #endregion

        #region [Ctor]
        public UserController(UserManager userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }
        #endregion

        #region [GET]
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            var users = await _userManager.FindAll().ToListAsync(cancellationToken);
            return Ok(_mapper.Map<IEnumerable<UserDTO>>(users));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
                return NotFound();

            return Ok(_mapper.Map<UserDTO>(user));
        }
        #endregion

        #region [POST]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserDTO dto, CancellationToken cancellationToken = default)
        {
            var user = _mapper.Map<User>(dto);
            _userManager.Add(user);
            await _userManager.SaveChangesAsync(cancellationToken);

            return Ok(_mapper.Map<UserDTO>(user));
        }
        #endregion

        #region [PUT]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UserDTO dto, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(dto.Id);
            if (user is null)
                return NotFound();

            _mapper.Map(dto, user);
            _userManager.Update(user);
            await _userManager.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
        #endregion

        #region [DELETE]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
                return NotFound();

            _userManager.Delete(user);
            await _userManager.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
        #endregion
    }
}
