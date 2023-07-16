using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearningSupportSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        #region [Fields]
        private readonly INotificationRepository _notificationRepository;
        private readonly IMapper _mapper;
        #endregion

        #region [Ctor]
        public NotificationController(INotificationRepository notificationRepository, IMapper mapper)
        {
            _notificationRepository = notificationRepository;
            _mapper = mapper;
        }
        #endregion

        #region [GET]
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            var notifications = await _notificationRepository.FindAll().ToListAsync(cancellationToken);
            return Ok(_mapper.Map<IEnumerable<NotificationDTO>>(notifications));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var notification = await _notificationRepository.FindByIdAsync(id);
            if (notification is null)
                return NotFound();

            return Ok(_mapper.Map<NotificationDTO>(notification));
        }
        #endregion

        #region [POST]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] NotificationDTO dto, CancellationToken cancellationToken = default)
        {
            var notification = _mapper.Map<Notification>(dto);
            _notificationRepository.Add(notification);
            await _notificationRepository.SaveChangesAsync(cancellationToken);

            return Ok(_mapper.Map<NotificationDTO>(notification));
        }
        #endregion

        #region [PUT]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] NotificationDTO dto, CancellationToken cancellationToken = default)
        {
            var notification = await _notificationRepository.FindByIdAsync(dto.Id, cancellationToken);
            if (notification is null)
                return NotFound();

            _mapper.Map(dto, notification);
            _notificationRepository.Update(notification);
            await _notificationRepository.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
        #endregion

        #region [DELETE]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
        {
            var notification = await _notificationRepository.FindByIdAsync(id, cancellationToken);
            if (notification is null)
                return NotFound();

            _notificationRepository.Delete(notification);
            await _notificationRepository.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
        #endregion
    }
}
