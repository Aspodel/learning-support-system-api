namespace LearningSupportSystemAPI;

public class Submission : BaseEntity
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? Description { get; set; }

    public string StudentId { get; set; } = string.Empty;
    public Student? Student { get; set; }

    public int? GroupId { get; set; }
    public Group? Group { get; set; }
    
    public int AssignmentId { get; set; }
    public Assignment? Assignment { get; set; }
}