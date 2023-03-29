using LearningSupportSystemAPI.Contract;
using LearningSupportSystemAPI.Core.Database;
using LearningSupportSystemAPI.Core.Entities;

namespace LearningSupportSystemAPI.Repository
{
    public class DiscussionRepository : BaseRepository<Discussion>, IDiscussionRepository
    {
        public DiscussionRepository(ApplicationDbContext context) : base(context) { }
    }
}
