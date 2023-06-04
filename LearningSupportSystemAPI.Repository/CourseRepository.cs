using LearningSupportSystemAPI.Contract;
using LearningSupportSystemAPI.Core.Database;
using LearningSupportSystemAPI.Core.Entities;
using LearningSupportSystemAPI.Repository.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LearningSupportSystemAPI.Repository
{
    public class CourseRepository : BaseRepository<Course>, ICourseRepository
    {
        public CourseRepository(ApplicationDbContext context) : base(context) { }
        
        public override IQueryable<Course> FindAll(Expression<Func<Course, bool>>? predicate = null)
            => _dbSet
                .WhereIf(predicate != null, predicate!)
                .Include(c => c.Classes);

        public override async Task<Course?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
            => await _dbSet
                    .Include(c => c.Classes)
                    .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }
}
