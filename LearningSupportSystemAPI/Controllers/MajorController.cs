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
    public class MajorController : ControllerBase
    {
        #region [Fields]
        private readonly IMajorRepository _majorRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;
        #endregion

        #region [Ctor]
        public MajorController(IMajorRepository majorRepository, IDepartmentRepository departmentRepository, IMapper mapper)
        {
            _majorRepository = majorRepository;
            _departmentRepository = departmentRepository;
            _mapper = mapper;
        }
        #endregion

        #region [GET]
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            var majors = await _majorRepository.FindAll().ToListAsync(cancellationToken);
            return Ok(_mapper.Map<IEnumerable<MajorDTO>>(majors));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var major = await _majorRepository.FindByIdAsync(id);
            if (major is null)
                return NotFound();

            return Ok(_mapper.Map<MajorDTO>(major));
        }
        #endregion

        #region [POST]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateMajorDTO dto, CancellationToken cancellationToken = default)
        {
            var department = await _departmentRepository.FindByIdAsync(dto.DepartmentId, cancellationToken);
            if (department is null)
                return NotFound();

            var major = _mapper.Map<Major>(dto);
            major.Department = department;

            _majorRepository.Add(major);
            await _majorRepository.SaveChangesAsync(cancellationToken);

            return Ok(_mapper.Map<MajorDTO>(major));
        }
        #endregion

        #region [PUT]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] MajorDTO dto, CancellationToken cancellationToken = default)
        {
            var major = await _majorRepository.FindByIdAsync(dto.Id, cancellationToken);
            if (major is null)
                return NotFound();

            _mapper.Map(dto, major);
            _majorRepository.Update(major);
            await _majorRepository.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
        #endregion

        #region [DELETE]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
        {
            var major = await _majorRepository.FindByIdAsync(id, cancellationToken);
            if (major is null)
                return NotFound();

            _majorRepository.Delete(major);
            await _majorRepository.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
        #endregion
    }
}
