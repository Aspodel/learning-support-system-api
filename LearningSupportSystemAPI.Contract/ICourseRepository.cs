using System.Linq.Expressions;

namespace LearningSupportSystemAPI;

public interface ICourseRepository : IBaseRepository<Course>
{
    IQueryable<Course> FindAllByDepartment(int departmentId, Expression<Func<Course, bool>>? predicate = null);
}
