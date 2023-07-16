namespace LearningSupportSystemAPI;

public class AnswerSubmission : Submission
{
    public int AnswerId { get; set; }
    public Answer? Answer { get; set; }

    public int QuestionId { get; set; }
    public Question? Question { get; set; }

}