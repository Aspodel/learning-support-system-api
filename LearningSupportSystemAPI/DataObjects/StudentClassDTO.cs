namespace LearningSupportSystemAPI;

public class StudentClassDTO
{
    public string StudentId { get; set; } = string.Empty;
    public int ClassId { get; set; }
    public int? GroupId { get; set; }

    public StudentDTO? Student { get; set; }
    public ClassDTO? Class { get; set; }
    public GroupDTO? Group { get; set; }

    public PositionType? Position { get; set; }
    public ICollection<ToDoItemDTO>? ToDoItems { get; set; }
}
