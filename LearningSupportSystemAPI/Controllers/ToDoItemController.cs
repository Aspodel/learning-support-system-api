using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearningSupportSystemAPI;


[Route("api/[controller]")]
[ApiController]
public class ToDoItemController : ControllerBase
{
    #region [Fields]
    private readonly IToDoItemRepository _toDoItemRepository;
    private readonly IGroupRepository _groupRepository;
    private readonly IToDoListRepository _toDoListRepository;
    private readonly StudentManager _studentManager;
    private readonly IMapper _mapper;
    #endregion

    #region [Ctor]
    public ToDoItemController(IToDoItemRepository toDoItemRepository, IMapper mapper, IGroupRepository groupRepository, IToDoListRepository toDoListRepository, StudentManager studentManager)
    {
        _toDoItemRepository = toDoItemRepository;
        _groupRepository = groupRepository;
        _toDoListRepository = toDoListRepository;
        _studentManager = studentManager;
        _mapper = mapper;
    }
    #endregion

    #region [GET]
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
    {
        var toDoItems = await _toDoItemRepository.FindAll().ToListAsync(cancellationToken);
        return Ok(_mapper.Map<IEnumerable<ToDoItemDTO>>(toDoItems));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var toDoItem = await _toDoItemRepository.FindByIdAsync(id);
        if (toDoItem is null)
            return NotFound();

        return Ok(_mapper.Map<ToDoItemDTO>(toDoItem));
    }

    [HttpGet("todolist/{toDoListId}")]
    public async Task<IActionResult> GetByToDoList(int toDoListId, CancellationToken cancellationToken = default)
    {
        var toDoItems = await _toDoItemRepository.FindAllByToDoList(toDoListId, cancellationToken).ToListAsync(cancellationToken);
        return Ok(_mapper.Map<IEnumerable<ToDoItemDTO>>(toDoItems));
    }
    #endregion

    #region [POST]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateToDoItemDTO dto, CancellationToken cancellationToken = default)
    {
        var toDoList = await _toDoListRepository.FindByIdAsync(dto.ToDoListId, cancellationToken);
        if (toDoList is null)
            return NotFound();

        var group = await _groupRepository.FindByIdAsync(toDoList.GroupId, cancellationToken);
        if (group is null)
            return NotFound();

        var toDoItem = _mapper.Map<ToDoItem>(dto);

        if (dto.Students != null)
        {
            foreach (var studentId in dto.Students)
            {
                var student = await _studentManager.FindByIdAsync(studentId);
                if (student is not null)
                    toDoItem.Students?.Add(new StudentTask { Student = student, ToDoItem = toDoItem });
            }
        }

        _toDoItemRepository.Add(toDoItem);
        await _toDoItemRepository.SaveChangesAsync(cancellationToken);

        return Ok(_mapper.Map<ToDoItemDTO>(toDoItem));
    }
    #endregion

    #region [PUT]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ToDoItemDTO dto, CancellationToken cancellationToken = default)
    {
        var toDoItem = await _toDoItemRepository.FindByIdAsync(dto.Id, cancellationToken);
        if (toDoItem is null)
            return NotFound();

        _mapper.Map(dto, toDoItem);
        _toDoItemRepository.Update(toDoItem);
        await _toDoItemRepository.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
    #endregion

    #region [DELETE]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
    {
        var toDoItem = await _toDoItemRepository.FindByIdAsync(id, cancellationToken);
        if (toDoItem is null)
            return NotFound();

        _toDoItemRepository.Delete(toDoItem);
        await _toDoItemRepository.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
    #endregion
}
