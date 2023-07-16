using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace LearningSupportSystemAPI;

public class FileSubmissionRepository : BaseRepository<FileSubmission>, IFileSubmissionRepository
{
    public FileSubmissionRepository(ApplicationDbContext context) : base(context) { }

    public override IQueryable<FileSubmission> FindAll(Expression<Func<FileSubmission, bool>>? predicate = null)
        => _dbSet
            .WhereIf(predicate != null, predicate!)
            .Include(f => f.Assignment)
            .Include(f => f.Student);
}