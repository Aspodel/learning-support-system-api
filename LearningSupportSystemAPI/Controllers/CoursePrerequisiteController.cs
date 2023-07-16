using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearningSupportSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursePrerequisiteController : ControllerBase
    {
        #region [Fields]
        private readonly ICoursePrerequisiteRepository _coursePrerequisiteRepository;
        private readonly IMapper _mapper;
        #endregion

        #region [Ctor]
        public CoursePrerequisiteController(ICoursePrerequisiteRepository coursePrerequisiteRepository, IMapper mapper)
        {
            _coursePrerequisiteRepository = coursePrerequisiteRepository;
            _mapper = mapper;
        }
        #endregion

        #region [GET]
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            var coursePrerequisites = await _coursePrerequisiteRepository.FindAll().ToListAsync(cancellationToken);
            return Ok(_mapper.Map<IEnumerable<CoursePrerequisiteDTO>>(coursePrerequisites));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var coursePrerequisite = await _coursePrerequisiteRepository.FindByIdAsync(id);
            if (coursePrerequisite is null)
                return NotFound();

            return Ok(_mapper.Map<CoursePrerequisiteDTO>(coursePrerequisite));
        }
        #endregion

        #region [POST]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CoursePrerequisiteDTO dto, CancellationToken cancellationToken = default)
        {
            var coursePrerequisite = _mapper.Map<CoursePrerequisite>(dto);
            _coursePrerequisiteRepository.Add(coursePrerequisite);
            await _coursePrerequisiteRepository.SaveChangesAsync(cancellationToken);

            return Ok(_mapper.Map<CoursePrerequisiteDTO>(coursePrerequisite));
        }
        #endregion

        #region [PUT]
        //[HttpPut]
        //public async Task<IActionResult> Update([FromBody] CoursePrerequisiteDTO dto, CancellationToken cancellationToken = default)
        //{
        //    var coursePrerequisite = await _coursePrerequisiteRepository.FindByIdAsync(dto.Id, cancellationToken);
        //    if (coursePrerequisite is null)
        //        return NotFound();

        //    _mapper.Map(dto, coursePrerequisite);
        //    _coursePrerequisiteRepository.Update(coursePrerequisite);
        //    await _coursePrerequisiteRepository.SaveChangesAsync(cancellationToken);

        //    return NoContent();
        //}
        #endregion

        #region [DELETE]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
        {
            var coursePrerequisite = await _coursePrerequisiteRepository.FindByIdAsync(id, cancellationToken);
            if (coursePrerequisite is null)
                return NotFound();

            _coursePrerequisiteRepository.Delete(coursePrerequisite);
            await _coursePrerequisiteRepository.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
        #endregion
    }
}
