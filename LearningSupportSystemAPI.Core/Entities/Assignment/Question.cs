namespace LearningSupportSystemAPI;

public class Question : BaseEntity
{
    public string Content { get; set; } = string.Empty;
    public QuestionType QuestionType { get; set; }
    public int Points { get; set; }

    public int AssignmentId { get; set; }
    public Assignment? Assignment { get; set; }

    public int? CorrectAnswerId { get; set; }
    public Answer? CorrectAnswer { get; set; }
    
    public ICollection<Answer>? Answers { get; set; }
}

public enum QuestionType
{
    MultipleChoice,
    ShortAnswer,
    LongAnswer,
}