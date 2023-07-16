namespace LearningSupportSystemAPI;

public class AnswerDTO : BaseDTO
{
    public string Content { get; set; } = string.Empty;
    public string? Explaination { get; set; }
    public int QuestionId { get; set; }
    public QuestionDTO? Question { get; set; }
}