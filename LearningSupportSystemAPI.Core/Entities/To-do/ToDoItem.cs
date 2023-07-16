namespace LearningSupportSystemAPI;

public class ToDoItem : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool? IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? Deadline { get; set; }
    public DateTime? CompletedAt { get; set; }

    public int ToDoListId { get; set; }
    public ToDoList? ToDoList { get; set; }

    public ICollection<StudentTask>? Students { get; set; }
}