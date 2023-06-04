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
    public class DepartmentController : ControllerBase
    {
        #region [Fields]
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;
        #endregion

        #region [Ctor]
        public DepartmentController(IDepartmentRepository departmentRepository, IMapper mapper)
        {
            _departmentRepository = departmentRepository;
            _mapper = mapper;
        }
        #endregion

        #region [GET]
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            var departments = await _departmentRepository.FindAll().ToListAsync(cancellationToken);
            return Ok(_mapper.Map<IEnumerable<DepartmentDTO>>(departments));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var department = await _departmentRepository.FindByIdAsync(id);
            if (department is null)
                return NotFound();

            return Ok(_mapper.Map<DepartmentDTO>(department));
        }
        #endregion

        #region [POST]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDepartmentDTO dto, CancellationToken cancellationToken = default)
        {
            var department = _mapper.Map<Department>(dto);
            _departmentRepository.Add(department);
            await _departmentRepository.SaveChangesAsync(cancellationToken);

            return Ok(_mapper.Map<DepartmentDTO>(department));
        }
        #endregion

        #region [PUT]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] DepartmentDTO dto, CancellationToken cancellationToken = default)
        {
            var department = await _departmentRepository.FindByIdAsync(dto.Id, cancellationToken);
            if (department is null)
                return NotFound();

            _mapper.Map(dto, department);
            _departmentRepository.Update(department);
            await _departmentRepository.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
        #endregion

        #region [DELETE]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
        {
            var department = await _departmentRepository.FindByIdAsync(id, cancellationToken);
            if (department is null)
                return NotFound();

            _departmentRepository.Delete(department);
            await _departmentRepository.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
        #endregion
    }
}
