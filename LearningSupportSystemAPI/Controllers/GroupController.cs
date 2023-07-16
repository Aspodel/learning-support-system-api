using AutoMapper;
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
        private readonly IClassRepository _classRepository;
        private readonly StudentManager _studentManager;
        private readonly IMapper _mapper;
        #endregion

        #region [Ctor]
        public GroupController(IGroupRepository groupRepository, IClassRepository classRepository, StudentManager studentManager, IMapper mapper)
        {
            _groupRepository = groupRepository;
            _classRepository = classRepository;
            _studentManager = studentManager;
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

        [HttpGet("class/{classId}")]
        public async Task<IActionResult> GetByClass(int classId, CancellationToken cancellationToken = default)
        {
            var cla = await _classRepository.FindByIdAsync(classId, cancellationToken);
            if (cla is null)
                return NotFound();

            var groups = await _groupRepository.FindAllByClass(classId).ToListAsync(cancellationToken);
            return Ok(_mapper.Map<IEnumerable<GroupDTO>>(groups));
        }
        #endregion

        #region [POST]
        [HttpPost("class/{classId}")]
        public async Task<IActionResult> Create(int classId, [FromBody] CreateGroupDTO dto, CancellationToken cancellationToken = default)
        {
            var cla = await _classRepository.FindByIdAsync(classId, cancellationToken);
            if (cla is null)
                return NotFound();

            var group = _mapper.Map<Group>(dto);
            group.Class = cla;
            group.Students = cla.Students
                .Where(sc => dto.Students.Contains(sc.StudentId))
                .ToList();

            // if (group.Students.Count != dto.Students.Count)
            // {
            //     return BadRequest("One or more student IDs are invalid.");
            // }

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
