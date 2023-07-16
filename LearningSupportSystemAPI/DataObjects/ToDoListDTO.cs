namespace LearningSupportSystemAPI;

public class ToDoListDTO : BaseDTO
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }

    public int GroupId { get; set; }
    public GroupDTO? Group { get; set; }

    public ICollection<ToDoItemDTO>? Items { get; set; }
}

public class CreateToDoListDTO
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }

    public int GroupId { get; set; }
}