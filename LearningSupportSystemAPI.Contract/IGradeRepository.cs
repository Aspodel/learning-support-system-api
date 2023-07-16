namespace LearningSupportSystemAPI;

public interface IGradeRepository : IBaseRepository<Grade>
{
    Task<Grade?> FindByDetail(string studentId, int gradeColumnId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Grade>> FindByStudent(string studentId, CancellationToken cancellationToken = default);
}
