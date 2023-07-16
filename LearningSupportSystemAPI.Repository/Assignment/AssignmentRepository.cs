using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace LearningSupportSystemAPI;

public class AssignmentRepository : BaseRepository<Assignment>, IAssignmentRepository
{
    public AssignmentRepository(ApplicationDbContext context) : base(context) { }

    public override IQueryable<Assignment> FindAll(Expression<Func<Assignment, bool>>? predicate = null)
        => _dbSet
            .WhereIf(predicate != null, predicate!)
            .Include(a => a.Class)
            .Include(a => a.Submissions);
}