using System.Linq.Expressions;

namespace LearningSupportSystemAPI.Repository.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> predicateIftrue)
              => condition ? query.Where(predicateIftrue) : query;
    }
}
