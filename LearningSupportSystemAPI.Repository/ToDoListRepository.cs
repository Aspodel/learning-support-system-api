using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace LearningSupportSystemAPI;

public class ToDoListRepository : BaseRepository<ToDoList>, IToDoListRepository
{
    public ToDoListRepository(ApplicationDbContext context) : base(context) { }

    public override IQueryable<ToDoList> FindAll(Expression<Func<ToDoList, bool>>? predicate = null)
        => _dbSet
            .AsNoTracking()
            .WhereIf(predicate != null, predicate!)
            .Include(x => x.Items);

    public IQueryable<ToDoList> FindAllByGroup(int groupId, CancellationToken cancellationToken = default)
        => _dbSet
            .AsNoTracking()
            .Where(x => x.GroupId == groupId)
            .Include(x => x.Items)
                .ThenInclude(x => x.Students)
                    .ThenInclude(x => x.Student);
}
