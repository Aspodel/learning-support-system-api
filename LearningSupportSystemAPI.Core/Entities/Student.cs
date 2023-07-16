namespace LearningSupportSystemAPI;

public class Student : User
{
    public int? StartYear { get; set; } = DateTime.UtcNow.Year;

    public int? MajorId { get; set; }
    public Major? Major { get; set; }

    public string? SupervisorId { get; set; } = string.Empty;
    public Lecturer? Supervisor { get; set; }

    public ICollection<StudentClass> RegisteredClasses { get; set; } = new HashSet<StudentClass>();
    public ICollection<Grade>? Grades { get; set; }
    public ICollection<StudentTask>? Tasks { get; set; }
}
