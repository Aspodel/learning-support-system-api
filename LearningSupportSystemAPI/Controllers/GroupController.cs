using AutoMapper;
using LearningSupportSystemAPI.Contract;
using LearningSupportSystemAPI.Core.Entities;
using LearningSupportSystemAPI.DataObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearningSupportSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        #region [Fields]
        private readonly IGroupRepository _groupRepository;
        private readonly IMapper _mapper;
        #endregion

        #region [Ctor]
        public GroupController(IGroupRepository groupRepository, IMapper mapper)
        {
            _groupRepository = groupRepository;
            _mapper = mapper;
        }
        #endregion

        #region [GET]
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            var groups = await _groupRepository.FindAll().ToListAsync(cancellationToken);
            return Ok(_mapper.Map<IEnumerable<GroupDTO>>(groups));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var group = await _groupRepository.FindByIdAsync(id);
            if (group is null)
                return NotFound();

            return Ok(_mapper.Map<GroupDTO>(group));
        }
        #endregion

        #region [POST]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] GroupDTO dto, CancellationToken cancellationToken = default)
        {
            var group = _mapper.Map<Group>(dto);
            _groupRepository.Add(group);
            await _groupRepository.SaveChangesAsync(cancellationToken);

            return Ok(_mapper.Map<GroupDTO>(group));
        }
        #endregion

        #region [PUT]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] GroupDTO dto, CancellationToken cancellationToken = default)
        {
            var group = await _groupRepository.FindByIdAsync(dto.Id, cancellationToken);
            if (group is null)
                return NotFound();

            _mapper.Map(dto, group);
            _groupRepository.Update(group);
            await _groupRepository.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
        #endregion

        #region [DELETE]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
        {
            var group = await _groupRepository.FindByIdAsync(id, cancellationToken);
            if (group is null)
                return NotFound();

            _groupRepository.Delete(group);
            await _groupRepository.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
        #endregion
    }
}
