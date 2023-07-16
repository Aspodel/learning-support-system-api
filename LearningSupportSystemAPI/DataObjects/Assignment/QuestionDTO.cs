namespace LearningSupportSystemAPI;

public class QuestionDTO : BaseDTO
{
    public string Content { get; set; } = string.Empty;
    public QuestionType QuestionType { get; set; } = QuestionType.MultipleChoice;
    public int Points { get; set; }

    public int AssignmentId { get; set; }
    public AssignmentDTO? Assignment { get; set; }

    public int? CorrectAnswerId { get; set; }
    public AnswerDTO? CorrectAnswer { get; set; }
    
    public ICollection<AnswerDTO>? Answers { get; set; }
}