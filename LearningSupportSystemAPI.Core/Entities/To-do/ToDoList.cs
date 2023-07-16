namespace LearningSupportSystemAPI;

public class ToDoList : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }

    public int GroupId { get; set; }
    public Group? Group { get; set; }

    public ICollection<ToDoItem>? Items { get; set; }
}
