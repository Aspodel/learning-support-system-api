namespace LearningSupportSystemAPI;

public class SemesterRepository : BaseRepository<Semester>, ISemesterRepository
{
    public SemesterRepository(ApplicationDbContext context) : base(context) { }
}
