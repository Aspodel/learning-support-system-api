namespace LearningSupportSystemAPI;

public class MajorRepository : BaseRepository<Major>, IMajorRepository
{
    public MajorRepository(ApplicationDbContext context) : base(context) { }
}
