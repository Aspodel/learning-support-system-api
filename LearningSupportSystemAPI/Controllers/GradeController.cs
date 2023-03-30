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
    public class GradeController : ControllerBase
    {
        #region [Fields]
        private readonly IGradeRepository _gradeRepository;
        private readonly IMapper _mapper;
        #endregion

        #region [Ctor]
        public GradeController(IGradeRepository gradeRepository, IMapper mapper)
        {
            _gradeRepository = gradeRepository;
            _mapper = mapper;
        }
        #endregion

        #region [GET]
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            var grades = await _gradeRepository.FindAll().ToListAsync(cancellationToken);
            return Ok(_mapper.Map<IEnumerable<GradeDTO>>(grades));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var grade = await _gradeRepository.FindByIdAsync(id);
            if (grade is null)
                return NotFound();

            return Ok(_mapper.Map<GradeDTO>(grade));
        }
        #endregion

        #region [POST]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] GradeDTO dto, CancellationToken cancellationToken = default)
        {
            var grade = _mapper.Map<Grade>(dto);
            _gradeRepository.Add(grade);
            await _gradeRepository.SaveChangesAsync(cancellationToken);

            return Ok(_mapper.Map<GradeDTO>(grade));
        }
        #endregion

        #region [PUT]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] GradeDTO dto, CancellationToken cancellationToken = default)
        {
            var grade = await _gradeRepository.FindByIdAsync(dto.Id, cancellationToken);
            if (grade is null)
                return NotFound();

            _mapper.Map(dto, grade);
            _gradeRepository.Update(grade);
            await _gradeRepository.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
        #endregion

        #region [DELETE]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
        {
            var grade = await _gradeRepository.FindByIdAsync(id, cancellationToken);
            if (grade is null)
                return NotFound();

            _gradeRepository.Delete(grade);
            await _gradeRepository.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
        #endregion
    }
}
