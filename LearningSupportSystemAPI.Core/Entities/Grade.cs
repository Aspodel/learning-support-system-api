namespace LearningSupportSystemAPI;

public class Grade : BaseEntity
{
    public int? Value { get; set; }

    public int GradeColumnId { get; set; }
    public GradeColumn? GradeColumn { get; set; }

    public string StudentId { get; set; } = string.Empty;
    public Student? Student { get; set; }
}
