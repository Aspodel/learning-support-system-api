using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearningSupportSystemAPI;

[Route("api/[controller]")]
[ApiController]
public class AssignmentController : ControllerBase
{
    #region [Fields]
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly IMapper _mapper;
    #endregion

    #region [Ctor]
    public AssignmentController(IAssignmentRepository assignmentRepository, IMapper mapper)
    {
        _assignmentRepository = assignmentRepository;
        _mapper = mapper;
    }
    #endregion

    #region [GET]
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
    {
        var assignments = await _assignmentRepository.FindAll().ToListAsync(cancellationToken);
        return Ok(_mapper.Map<IEnumerable<AssignmentDTO>>(assignments));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var assignment = await _assignmentRepository.FindByIdAsync(id);
        if (assignment is null)
            return NotFound();

        return Ok(_mapper.Map<AssignmentDTO>(assignment));
    }

    [HttpGet("class/{classId}")]
    public async Task<IActionResult> GetByClass(int classId, CancellationToken cancellationToken = default)
    {
        var assignments = await _assignmentRepository.FindAll(a => a.ClassId == classId).ToListAsync(cancellationToken);
        return Ok(_mapper.Map<IEnumerable<AssignmentDTO>>(assignments));
    }
    #endregion

    #region [POST]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAssignmentDTO dto, CancellationToken cancellationToken = default)
    {
        var assignment = _mapper.Map<Assignment>(dto);
        _assignmentRepository.Add(assignment);
        await _assignmentRepository.SaveChangesAsync(cancellationToken);

        return Ok(_mapper.Map<AssignmentDTO>(assignment));
    }
    #endregion

    #region [PUT]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] AssignmentDTO dto, CancellationToken cancellationToken = default)
    {
        var assignment = await _assignmentRepository.FindByIdAsync(dto.Id, cancellationToken);
        if (assignment is null)
            return NotFound();

        _mapper.Map(dto, assignment);
        _assignmentRepository.Update(assignment);
        await _assignmentRepository.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
    #endregion

    #region [DELETE]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
    {
        var assignment = await _assignmentRepository.FindByIdAsync(id, cancellationToken);
        if (assignment is null)
            return NotFound();

        _assignmentRepository.Delete(assignment);
        await _assignmentRepository.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
    #endregion
}
