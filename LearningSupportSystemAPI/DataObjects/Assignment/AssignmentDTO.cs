namespace LearningSupportSystemAPI;

public class AssignmentDTO : BaseDTO
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public AssignmentType AssignmentType { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? PublishDate { get; set; }
    public DateTime? AnswerPublishDate { get; set; }

    public int ClassId { get; set; }
    public int? GradeColumnId { get; set; }
    public ICollection<QuestionDTO>? Questions { get; set; }
    public ICollection<SubmissionDTO>? Submissions { get; set; }
}

public class CreateAssignmentDTO : BaseDTO
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public AssignmentType AssignmentType { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? PublishDate { get; set; }
    public DateTime? AnswerPublishDate { get; set; }

    public int ClassId { get; set; }
    public int? GradeColumnId { get; set; }
    public ICollection<QuestionDTO>? Questions { get; set; }
}