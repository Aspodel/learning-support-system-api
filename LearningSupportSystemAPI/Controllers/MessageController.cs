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
    public class MessageController : ControllerBase
    {
        #region [Fields]
        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;
        #endregion

        #region [Ctor]
        public MessageController(IMessageRepository messageRepository, IMapper mapper)
        {
            _messageRepository = messageRepository;
            _mapper = mapper;
        }
        #endregion

        #region [GET]
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            var messages = await _messageRepository.FindAll().ToListAsync(cancellationToken);
            return Ok(_mapper.Map<IEnumerable<MessageDTO>>(messages));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var message = await _messageRepository.FindByIdAsync(id);
            if (message is null)
                return NotFound();

            return Ok(_mapper.Map<MessageDTO>(message));
        }
        #endregion

        #region [POST]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MessageDTO dto, CancellationToken cancellationToken = default)
        {
            var message = _mapper.Map<Message>(dto);
            _messageRepository.Add(message);
            await _messageRepository.SaveChangesAsync(cancellationToken);

            return Ok(_mapper.Map<MessageDTO>(message));
        }
        #endregion

        #region [PUT]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] MessageDTO dto, CancellationToken cancellationToken = default)
        {
            var message = await _messageRepository.FindByIdAsync(dto.Id, cancellationToken);
            if (message is null)
                return NotFound();

            _mapper.Map(dto, message);
            _messageRepository.Update(message);
            await _messageRepository.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
        #endregion

        #region [DELETE]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
        {
            var message = await _messageRepository.FindByIdAsync(id, cancellationToken);
            if (message is null)
                return NotFound();

            _messageRepository.Delete(message);
            await _messageRepository.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
        #endregion
    }
}
