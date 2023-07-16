using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LearningSupportSystemAPI;

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

    public IQueryable<Course> FindAllByDepartment(int departmentId, Expression<Func<Course, bool>>? predicate = null)
        => FindAll(predicate)
            .Where(c => c.DepartmentId == departmentId);
}
