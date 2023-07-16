namespace LearningSupportSystemAPI;

public class StudentTask : BaseEntity
{
    public int ToDoItemId { get; set; }
    public ToDoItem? ToDoItem { get; set; }

    public string StudentId { get; set; } = string.Empty;
    public Student? Student { get; set; }
}