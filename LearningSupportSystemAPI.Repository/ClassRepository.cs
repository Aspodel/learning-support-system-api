using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace LearningSupportSystemAPI;

public class ClassRepository : BaseRepository<Class>, IClassRepository
{
    public ClassRepository(ApplicationDbContext context) : base(context) { }

    public override IQueryable<Class> FindAll(Expression<Func<Class, bool>>? predicate = null)
        => _dbSet
            .WhereIf(predicate != null, predicate!)
            .Include(c => c.Course)
            .Include(c => c.Students)
                .ThenInclude(s => s.Student)
            .Include(c => c.GradeColumns);

    public override Task<Class?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        => FindAll(c => c.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
}
