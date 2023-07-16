using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace LearningSupportSystemAPI;

public class GroupRepository : BaseRepository<Group>, IGroupRepository
{
    public GroupRepository(ApplicationDbContext context) : base(context) { }

    public override IQueryable<Group> FindAll(Expression<Func<Group, bool>>? predicate = null)
        => _dbSet
            .WhereIf(predicate != null, predicate!)
            .Include(x => x.Students)
                .ThenInclude(x => x.Student);

    public override Task<Group?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        => FindAll(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

    public IQueryable<Group> FindAllByClass(int classId)
        => FindAll(x => x.ClassId == classId);
}
