namespace LearningSupportSystemAPI;

public class Assignment : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public AssignmentType AssignmentType { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DueDate { get; set; }
    public DateTime? PublishDate { get; set; }
    public DateTime? AnswerPublishDate { get; set; }


    public int ClassId { get; set; }
    public Class? Class { get; set; }

    public int? GradeColumnId { get; set; }
    public GradeColumn? GradeColumn { get; set; }
    
    public ICollection<Submission>? Submissions { get; set; }
    public ICollection<Question>? Questions { get; set; }
}

public enum AssignmentType
{
    File,
    Question
}
