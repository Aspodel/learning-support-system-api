using System.Linq.Expressions;

namespace LearningSupportSystemAPI;

public interface IGradeColumnRepository : IBaseRepository<GradeColumn>
{
    IQueryable<GradeColumn> FindAllByClass(int classId, Expression<Func<GradeColumn, bool>>? predicate = null);
}
