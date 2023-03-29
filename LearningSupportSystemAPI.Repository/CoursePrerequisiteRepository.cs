using LearningSupportSystemAPI.Contract;
using LearningSupportSystemAPI.Core.Database;
using LearningSupportSystemAPI.Core.Entities;

namespace LearningSupportSystemAPI.Repository
{
    public class CoursePrerequisiteRepository : BaseRepository<CoursePrerequisite>, ICoursePrerequisiteRepository
    {
        public CoursePrerequisiteRepository(ApplicationDbContext context) : base(context) { }
    }
}
