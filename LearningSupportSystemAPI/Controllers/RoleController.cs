using AutoMapper;
using LearningSupportSystemAPI.Core.Entities;
using LearningSupportSystemAPI.DataObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearningSupportSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        #region [Fields]
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;
        #endregion

        #region [Ctor]
        public RoleController(RoleManager<Role> roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }
        #endregion

        #region [GET]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var roles = await _roleManager.Roles.OrderBy(r => r.NormalizedName).ToListAsync();
            return Ok(_mapper.Map<IEnumerable<RoleDTO>>(roles));
        }
        #endregion

        #region [POST]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RoleDTO dTO)
        {
            var role = _mapper.Map<Role>(dTO);
            await _roleManager.CreateAsync(role);

            return Ok(_mapper.Map<RoleDTO>(role));
        }
        #endregion

        #region [PUT]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] RoleDTO dTO)
        {
            var role = await _roleManager.FindByIdAsync(dTO.Id);
            if (role is null)
                return NotFound();

            _mapper.Map(dTO, role);
            await _roleManager.UpdateAsync(role);
            return NoContent();
        }
        #endregion

        #region [DELETE]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role is null)
                return NotFound();

            await _roleManager.DeleteAsync(role);
            return NoContent();
        }
        #endregion
    }
}
