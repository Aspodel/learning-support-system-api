using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearningSupportSystemAPI;


[Route("api/[controller]")]
[ApiController]
public class ToDoListController : ControllerBase
{
    #region [Fields]
    private readonly IToDoListRepository _toDoListRepository;
    private readonly IMapper _mapper;
    #endregion

    #region [Ctor]
    public ToDoListController(IToDoListRepository toDoListRepository, IMapper mapper)
    {
        _toDoListRepository = toDoListRepository;
        _mapper = mapper;
    }
    #endregion

    #region [GET]
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
    {
        var toDoLists = await _toDoListRepository.FindAll().ToListAsync(cancellationToken);
        return Ok(_mapper.Map<IEnumerable<ToDoListDTO>>(toDoLists));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var toDoList = await _toDoListRepository.FindByIdAsync(id);
        if (toDoList is null)
            return NotFound();

        return Ok(_mapper.Map<ToDoListDTO>(toDoList));
    }

    [HttpGet("group/{groupId}")]
    public async Task<IActionResult> GetByGroup(int groupId, CancellationToken cancellationToken = default)
    {
        var toDoLists = await _toDoListRepository.FindAllByGroup(groupId, cancellationToken).ToListAsync(cancellationToken);
        return Ok(_mapper.Map<IEnumerable<ToDoListDTO>>(toDoLists));
    }
    #endregion

    #region [POST]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateToDoListDTO dto, CancellationToken cancellationToken = default)
    {
        var toDoList = _mapper.Map<ToDoList>(dto);
        _toDoListRepository.Add(toDoList);
        await _toDoListRepository.SaveChangesAsync(cancellationToken);

        return Ok(_mapper.Map<ToDoListDTO>(toDoList));
    }
    #endregion

    #region [PUT]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ToDoListDTO dto, CancellationToken cancellationToken = default)
    {
        var toDoList = await _toDoListRepository.FindByIdAsync(dto.Id, cancellationToken);
        if (toDoList is null)
            return NotFound();

        _mapper.Map(dto, toDoList);
        _toDoListRepository.Update(toDoList);
        await _toDoListRepository.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
    #endregion

    #region [DELETE]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
    {
        var toDoList = await _toDoListRepository.FindByIdAsync(id, cancellationToken);
        if (toDoList is null)
            return NotFound();

        _toDoListRepository.Delete(toDoList);
        await _toDoListRepository.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
    #endregion
}
