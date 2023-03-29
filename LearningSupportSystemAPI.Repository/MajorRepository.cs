using LearningSupportSystemAPI.Contract;
using LearningSupportSystemAPI.Core.Database;
using LearningSupportSystemAPI.Core.Entities;

namespace LearningSupportSystemAPI.Repository
{
    public class MajorRepository : BaseRepository<Major>, IMajorRepository
    {
        public MajorRepository(ApplicationDbContext context) : base(context) { }
    }
}
