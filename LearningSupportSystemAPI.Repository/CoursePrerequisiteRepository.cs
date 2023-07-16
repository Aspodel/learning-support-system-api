namespace LearningSupportSystemAPI;

public class CoursePrerequisiteRepository : BaseRepository<CoursePrerequisite>, ICoursePrerequisiteRepository
{
    public CoursePrerequisiteRepository(ApplicationDbContext context) : base(context) { }
}
