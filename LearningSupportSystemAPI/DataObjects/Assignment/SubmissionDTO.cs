namespace LearningSupportSystemAPI;

public class SubmissionDTO : BaseDTO
{
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    public string? Description { get; set; }

    public string StudentId { get; set; } = string.Empty;
    public int? GroupId { get; set; }
    public int AssignmentId { get; set; }
}

public class FileSubmissionDTO : SubmissionDTO
{
    public string FileName { get; set; } = string.Empty;
    public string? FileUrl { get; set; }
}

public class AnswerSubmissionDTO : SubmissionDTO
{
    public int AnswerId { get; set; }
    public AnswerDTO? Answer { get; set; }

    public int QuestionId { get; set; }
    public QuestionDTO? Question { get; set; }
}

public class CreateFileSubmissionDTO : SubmissionDTO
{
    public string FileName { get; set; } = string.Empty;
    public string? FileUrl { get; set; }
    public IFormFile File { get; set; } = null!;
}