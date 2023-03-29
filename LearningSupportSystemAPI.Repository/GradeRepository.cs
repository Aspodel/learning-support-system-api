using LearningSupportSystemAPI.Contract;
using LearningSupportSystemAPI.Core.Database;
using LearningSupportSystemAPI.Core.Entities;

namespace LearningSupportSystemAPI.Repository
{
    public class GradeRepository : BaseRepository<Grade>, IGradeRepository
    {
        public GradeRepository(ApplicationDbContext context) : base(context) { }
    }
}
