namespace LearningSupportSystemAPI;

public class ToDoItemDTO : BaseDTO
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool? IsCompleted { get; set; } = false;
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? Deadline { get; set; }
    public DateTime? CompletedAt { get; set; }

    public int ToDoListId { get; set; }
    public ToDoListDTO? ToDoList { get; set; }

    public ICollection<StudentTaskDTO>? Students { get; set; }
}

public class CreateToDoItemDTO : BaseDTO
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool? IsCompleted { get; set; } = false;
    public DateTime? Deadline { get; set; }

    public int ToDoListId { get; set; }
    public ICollection<string>? Students { get; set; }
}