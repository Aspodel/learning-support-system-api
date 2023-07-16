namespace LearningSupportSystemAPI;

public class Answer : BaseEntity
{
    public string Content { get; set; } = string.Empty;
    public string? Explaination { get; set; }
    public int QuestionId { get; set; }
    public Question? Question { get; set; }
}