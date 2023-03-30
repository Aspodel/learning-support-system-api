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
    public class RoomController : ControllerBase
    {
        #region [Fields]
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;
        #endregion

        #region [Ctor]
        public RoomController(IRoomRepository roomRepository, IMapper mapper)
        {
            _roomRepository = roomRepository;
            _mapper = mapper;
        }
        #endregion

        #region [GET]
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            var rooms = await _roomRepository.FindAll().ToListAsync(cancellationToken);
            return Ok(_mapper.Map<IEnumerable<RoomDTO>>(rooms));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var room = await _roomRepository.FindByIdAsync(id);
            if (room is null)
                return NotFound();

            return Ok(_mapper.Map<RoomDTO>(room));
        }
        #endregion

        #region [POST]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RoomDTO dto, CancellationToken cancellationToken = default)
        {
            var room = _mapper.Map<Room>(dto);
            _roomRepository.Add(room);
            await _roomRepository.SaveChangesAsync(cancellationToken);

            return Ok(_mapper.Map<RoomDTO>(room));
        }
        #endregion

        #region [PUT]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] RoomDTO dto, CancellationToken cancellationToken = default)
        {
            var room = await _roomRepository.FindByIdAsync(dto.Id, cancellationToken);
            if (room is null)
                return NotFound();

            _mapper.Map(dto, room);
            _roomRepository.Update(room);
            await _roomRepository.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
        #endregion

        #region [DELETE]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
        {
            var room = await _roomRepository.FindByIdAsync(id, cancellationToken);
            if (room is null)
                return NotFound();

            _roomRepository.Delete(room);
            await _roomRepository.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
        #endregion
    }
}
