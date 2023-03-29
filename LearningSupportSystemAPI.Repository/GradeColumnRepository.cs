using LearningSupportSystemAPI.Contract;
using LearningSupportSystemAPI.Core.Database;
using LearningSupportSystemAPI.Core.Entities;

namespace LearningSupportSystemAPI.Repository
{
    public class GradeColumnRepository : BaseRepository<GradeColumn>, IGradeColumnRepository
    {
        public GradeColumnRepository(ApplicationDbContext context) : base(context) { }
    }
}
