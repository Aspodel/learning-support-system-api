using Microsoft.EntityFrameworkCore;

namespace LearningSupportSystemAPI;

public class GradeRepository : BaseRepository<Grade>, IGradeRepository
{
    public GradeRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Grade?> FindByDetail(string studentId, int gradeColumnId, CancellationToken cancellationToken = default)
        => await _dbSet
            // .AsNoTracking()
            .FirstOrDefaultAsync(g => g.StudentId == studentId && g.GradeColumnId == gradeColumnId, cancellationToken);

    public async Task<IEnumerable<Grade>> FindByStudent(string studentId, CancellationToken cancellationToken = default)
        => await _dbSet
            // .AsNoTracking()
            .Where(g => g.StudentId == studentId)
            .Include(g => g.GradeColumn)
                .ThenInclude(gc => gc!.Class)
                .ThenInclude(c => c!.Course)
            .ToListAsync(cancellationToken);
}
