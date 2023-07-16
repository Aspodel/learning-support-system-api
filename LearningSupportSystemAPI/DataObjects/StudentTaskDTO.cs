namespace LearningSupportSystemAPI;

public class StudentTaskDTO
{
    public int ToDoItemId { get; set; }
    public ToDoItemDTO? ToDoItem { get; set; }
    
    public string StudentId { get; set; } = string.Empty;
    public StudentDTO? Student { get; set; }
}