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
    public class GradeColumnController : ControllerBase
    {
        #region [Fields]
        private readonly IGradeColumnRepository _gradeColumnRepository;
        private readonly IMapper _mapper;
        #endregion

        #region [Ctor]
        public GradeColumnController(IGradeColumnRepository gradeColumnRepository, IMapper mapper)
        {
            _gradeColumnRepository = gradeColumnRepository;
            _mapper = mapper;
        }
        #endregion

        #region [GET]
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            var gradeColumns = await _gradeColumnRepository.FindAll().ToListAsync(cancellationToken);
            return Ok(_mapper.Map<IEnumerable<GradeColumnDTO>>(gradeColumns));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var gradeColumn = await _gradeColumnRepository.FindByIdAsync(id);
            if (gradeColumn is null)
                return NotFound();

            return Ok(_mapper.Map<GradeColumnDTO>(gradeColumn));
        }
        #endregion

        #region [POST]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] GradeColumnDTO dto, CancellationToken cancellationToken = default)
        {
            var gradeColumn = _mapper.Map<GradeColumn>(dto);
            _gradeColumnRepository.Add(gradeColumn);
            await _gradeColumnRepository.SaveChangesAsync(cancellationToken);

            return Ok(_mapper.Map<GradeColumnDTO>(gradeColumn));
        }
        #endregion

        #region [PUT]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] GradeColumnDTO dto, CancellationToken cancellationToken = default)
        {
            var gradeColumn = await _gradeColumnRepository.FindByIdAsync(dto.Id, cancellationToken);
            if (gradeColumn is null)
                return NotFound();

            _mapper.Map(dto, gradeColumn);
            _gradeColumnRepository.Update(gradeColumn);
            await _gradeColumnRepository.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
        #endregion

        #region [DELETE]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
        {
            var gradeColumn = await _gradeColumnRepository.FindByIdAsync(id, cancellationToken);
            if (gradeColumn is null)
                return NotFound();

            _gradeColumnRepository.Delete(gradeColumn);
            await _gradeColumnRepository.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
        #endregion
    }
}
