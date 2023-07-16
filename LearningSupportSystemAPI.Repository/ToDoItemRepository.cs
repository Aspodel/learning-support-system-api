using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace LearningSupportSystemAPI;

public class ToDoItemRepository : BaseRepository<ToDoItem>, IToDoItemRepository
{
    public ToDoItemRepository(ApplicationDbContext context) : base(context) { }

    public override IQueryable<ToDoItem> FindAll(Expression<Func<ToDoItem, bool>>? predicate = null)
        => _dbSet
            .WhereIf(predicate != null, predicate!)
            .Include(x => x.Students);

    public override async Task<ToDoItem?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        => await _dbSet
            .AsNoTracking()
            .Include(x => x.Students)
                .ThenInclude(x => x.Student)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public IQueryable<ToDoItem> FindAllByToDoList(int toDoListId, CancellationToken cancellationToken = default)
        => _dbSet
            .AsNoTracking()
            .Where(x => x.ToDoListId == toDoListId)
            .Include(x => x.Students)
                .ThenInclude(x => x.Student);
}