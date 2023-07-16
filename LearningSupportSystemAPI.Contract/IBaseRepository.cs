using System.Linq.Expressions;

namespace LearningSupportSystemAPI;

public interface IBaseRepository<T> where T : class
{
    IQueryable<T> FindAll(Expression<Func<T, bool>>? predicate = null);
    Task<T?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
    void Add(T entity);
    void AddRange(IEnumerable<T> entities);
    void Update(T entity);
    void UpdateRange(IEnumerable<T> entities);
    void Delete(T entity);
    void DeleteRange(IEnumerable<T> entities);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
