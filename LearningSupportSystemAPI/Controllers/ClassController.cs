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
    public class ClassController : ControllerBase
    {
        #region [Fields]
        private readonly IClassRepository _classRepository;
        private readonly IMapper _mapper;
        #endregion

        #region [Ctor]
        public ClassController(IClassRepository classRepository, IMapper mapper)
        {
            _classRepository = classRepository;
            _mapper = mapper;
        }
        #endregion

        #region [GET]
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            var classes = await _classRepository.FindAll().ToListAsync(cancellationToken);
            return Ok(_mapper.Map<IEnumerable<ClassDTO>>(classes));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var cla = await _classRepository.FindByIdAsync(id);
            if (cla is null)
                return NotFound();

            return Ok(_mapper.Map<ClassDTO>(cla));
        }
        #endregion

        #region [POST]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ClassDTO dto, CancellationToken cancellationToken = default)
        {
            var cla = _mapper.Map<Class>(dto);
            _classRepository.Add(cla);
            await _classRepository.SaveChangesAsync(cancellationToken);

            return Ok(_mapper.Map<ClassDTO>(cla));
        }
        #endregion

        #region [PUT]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ClassDTO dto, CancellationToken cancellationToken = default)
        {
            var cla = await _classRepository.FindByIdAsync(dto.Id, cancellationToken);
            if (cla is null)
                return NotFound();

            _mapper.Map(dto, cla);
            _classRepository.Update(cla);
            await _classRepository.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
        #endregion

        #region [DELETE]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
        {
            var cla = await _classRepository.FindByIdAsync(id, cancellationToken);
            if (cla is null)
                return NotFound();

            _classRepository.Delete(cla);
            await _classRepository.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
        #endregion
    }
}
