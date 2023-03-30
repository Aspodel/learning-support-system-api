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
    public class DiscussionController : ControllerBase
    {
        #region [Fields]
        private readonly IDiscussionRepository _discussionRepository;
        private readonly IMapper _mapper;
        #endregion

        #region [Ctor]
        public DiscussionController(IDiscussionRepository discussionRepository, IMapper mapper)
        {
            _discussionRepository = discussionRepository;
            _mapper = mapper;
        }
        #endregion

        #region [GET]
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            var discussions = await _discussionRepository.FindAll().ToListAsync(cancellationToken);
            return Ok(_mapper.Map<IEnumerable<DepartmentDTO>>(discussions));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var discussion = await _discussionRepository.FindByIdAsync(id);
            if (discussion is null)
                return NotFound();

            return Ok(_mapper.Map<DepartmentDTO>(discussion));
        }
        #endregion

        #region [POST]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DepartmentDTO dto, CancellationToken cancellationToken = default)
        {
            var discussion = _mapper.Map<Department>(dto);
            _discussionRepository.Add(discussion);
            await _discussionRepository.SaveChangesAsync(cancellationToken);

            return Ok(_mapper.Map<DepartmentDTO>(discussion));
        }
        #endregion

        #region [PUT]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] DepartmentDTO dto, CancellationToken cancellationToken = default)
        {
            var discussion = await _discussionRepository.FindByIdAsync(dto.Id, cancellationToken);
            if (discussion is null)
                return NotFound();

            _mapper.Map(dto, discussion);
            _discussionRepository.Update(discussion);
            await _discussionRepository.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
        #endregion

        #region [DELETE]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
        {
            var discussion = await _discussionRepository.FindByIdAsync(id, cancellationToken);
            if (discussion is null)
                return NotFound();

            _discussionRepository.Delete(discussion);
            await _discussionRepository.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
        #endregion
    }
}
