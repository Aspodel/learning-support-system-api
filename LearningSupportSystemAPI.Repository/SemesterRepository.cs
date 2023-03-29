using LearningSupportSystemAPI.Contract;
using LearningSupportSystemAPI.Core.Database;
using LearningSupportSystemAPI.Core.Entities;

namespace LearningSupportSystemAPI.Repository
{
    public class SemesterRepository : BaseRepository<Semester>, ISemesterRepository
    {
        public SemesterRepository(ApplicationDbContext context) : base(context) { }
    }
}
