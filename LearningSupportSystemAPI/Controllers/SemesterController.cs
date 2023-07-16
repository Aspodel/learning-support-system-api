using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearningSupportSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SemesterController : ControllerBase
    {
        #region [Fields]
        private readonly ISemesterRepository _semesterRepository;
        private readonly IMapper _mapper;
        #endregion

        #region [Ctor]
        public SemesterController(ISemesterRepository semesterRepository, IMapper mapper)
        {
            _semesterRepository = semesterRepository;
            _mapper = mapper;
        }
        #endregion

        #region [GET]
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            var semesters = await _semesterRepository.FindAll().ToListAsync(cancellationToken);
            return Ok(_mapper.Map<IEnumerable<SemesterDTO>>(semesters));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var semester = await _semesterRepository.FindByIdAsync(id);
            if (semester is null)
                return NotFound();

            return Ok(_mapper.Map<SemesterDTO>(semester));
        }
        #endregion

        #region [POST]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SemesterDTO dto, CancellationToken cancellationToken = default)
        {
            var semester = _mapper.Map<Semester>(dto);
            _semesterRepository.Add(semester);
            await _semesterRepository.SaveChangesAsync(cancellationToken);

            return Ok(_mapper.Map<SemesterDTO>(semester));
        }
        #endregion

        #region [PUT]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] SemesterDTO dto, CancellationToken cancellationToken = default)
        {
            var semester = await _semesterRepository.FindByIdAsync(dto.Id, cancellationToken);
            if (semester is null)
                return NotFound();

            _mapper.Map(dto, semester);
            _semesterRepository.Update(semester);
            await _semesterRepository.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
        #endregion

        #region [DELETE]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
        {
            var semester = await _semesterRepository.FindByIdAsync(id, cancellationToken);
            if (semester is null)
                return NotFound();

            _semesterRepository.Delete(semester);
            await _semesterRepository.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
        #endregion
    }
}
