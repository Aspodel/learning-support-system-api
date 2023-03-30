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
    public class CourseController : ControllerBase
    {
        #region [Fields]
        private readonly ICourseRepository _courseRepository;
        private readonly IMapper _mapper;
        #endregion

        #region [Ctor]
        public CourseController(ICourseRepository courseRepository, IMapper mapper)
        {
            _courseRepository = courseRepository;
            _mapper = mapper;
        }
        #endregion

        #region [GET]
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            var courses = await _courseRepository.FindAll().ToListAsync(cancellationToken);
            return Ok(_mapper.Map<IEnumerable<CourseDTO>>(courses));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var course = await _courseRepository.FindByIdAsync(id);
            if (course is null)
                return NotFound();

            return Ok(_mapper.Map<CourseDTO>(course));
        }
        #endregion

        #region [POST]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CourseDTO dto, CancellationToken cancellationToken = default)
        {
            var course = _mapper.Map<Course>(dto);
            _courseRepository.Add(course);
            await _courseRepository.SaveChangesAsync(cancellationToken);

            return Ok(_mapper.Map<CourseDTO>(course));
        }
        #endregion

        #region [PUT]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CourseDTO dto, CancellationToken cancellationToken = default)
        {
            var course = await _courseRepository.FindByIdAsync(dto.Id, cancellationToken);
            if (course is null)
                return NotFound();

            _mapper.Map(dto, course);
            _courseRepository.Update(course);
            await _courseRepository.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
        #endregion

        #region [DELETE]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
        {
            var course = await _courseRepository.FindByIdAsync(id, cancellationToken);
            if (course is null)
                return NotFound();

            _courseRepository.Delete(course);
            await _courseRepository.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
        #endregion
    }
}
