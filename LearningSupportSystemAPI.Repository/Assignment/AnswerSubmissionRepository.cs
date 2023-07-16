namespace LearningSupportSystemAPI;

public class AnswerSubmissionRepository : BaseRepository<AnswerSubmission>, IAnswerSubmissionRepository
{
    public AnswerSubmissionRepository(ApplicationDbContext context) : base(context) { }
}