using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace LearningSupportSystemAPI;

public class DiscussionRepository : BaseRepository<Discussion>, IDiscussionRepository
{
    public DiscussionRepository(ApplicationDbContext context) : base(context) { }

    public override IQueryable<Discussion> FindAll(Expression<Func<Discussion, bool>>? predicate = null)
        => _dbSet
            .Include(d => d.Creator)
            .Include(d => d.Messages)
                .ThenInclude(m => m.Sender)
            .Where(predicate ?? (_ => true));

    public override Task<Discussion?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        => _dbSet
            .Include(d => d.Creator)
            .Include(d => d.Messages)
                .ThenInclude(m => m.Sender)
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
}
