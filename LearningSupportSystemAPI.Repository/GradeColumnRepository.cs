using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace LearningSupportSystemAPI;

public class GradeColumnRepository : BaseRepository<GradeColumn>, IGradeColumnRepository
{
    public GradeColumnRepository(ApplicationDbContext context) : base(context) { }

    public override IQueryable<GradeColumn> FindAll(Expression<Func<GradeColumn, bool>>? predicate = null)
        => _dbSet
            .WhereIf(predicate != null, predicate!)
            .Include(c => c.Grades);

    public IQueryable<GradeColumn> FindAllByClass(int classId, Expression<Func<GradeColumn, bool>>? predicate = null)
        => FindAll(predicate)
            .Where(c => c.ClassId == classId);
}
