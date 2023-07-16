using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearningSupportSystemAPI;

[Route("api/[controller]")]
[ApiController]
public class AnnouncementController : ControllerBase
{
    #region [Fields]
    private readonly IAnnouncementRepository _announcementRepository;
    private readonly IMapper _mapper;
    #endregion

    #region [Ctor]
    public AnnouncementController(IAnnouncementRepository announcementRepository, IMapper mapper)
    {
        _announcementRepository = announcementRepository;
        _mapper = mapper;
    }
    #endregion

    #region [GET]
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
    {
        var announcements = await _announcementRepository.FindAll().ToListAsync(cancellationToken);
        return Ok(_mapper.Map<IEnumerable<AnnouncementDTO>>(announcements));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var announcement = await _announcementRepository.FindByIdAsync(id);
        if (announcement is null)
            return NotFound();

        return Ok(_mapper.Map<AnnouncementDTO>(announcement));
    }
    #endregion

    #region [POST]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AnnouncementDTO dto, CancellationToken cancellationToken = default)
    {
        var announcement = _mapper.Map<Announcement>(dto);
        _announcementRepository.Add(announcement);
        await _announcementRepository.SaveChangesAsync(cancellationToken);

        return Ok(_mapper.Map<AnnouncementDTO>(announcement));
    }
    #endregion

    #region [PUT]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] AnnouncementDTO dto, CancellationToken cancellationToken = default)
    {
        var announcement = await _announcementRepository.FindByIdAsync(dto.Id, cancellationToken);
        if (announcement is null)
            return NotFound();

        _mapper.Map(dto, announcement);
        _announcementRepository.Update(announcement);
        await _announcementRepository.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
    #endregion

    #region [DELETE]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
    {
        var announcement = await _announcementRepository.FindByIdAsync(id, cancellationToken);
        if (announcement is null)
            return NotFound();

        _announcementRepository.Delete(announcement);
        await _announcementRepository.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
    #endregion
}
