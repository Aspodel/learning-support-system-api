namespace LearningSupportSystemAPI;

public class Group : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string ProjectName { get; set; } = string.Empty;

    public int ClassId { get; set; }
    public Class? Class { get; set; }

    public ICollection<StudentClass> Students { get; set; } = new HashSet<StudentClass>();
    public ICollection<ToDoList> ToDoLists { get; set; } = new HashSet<ToDoList>();
}
