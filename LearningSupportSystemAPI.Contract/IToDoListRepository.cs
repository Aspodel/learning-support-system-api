namespace LearningSupportSystemAPI;
public interface IToDoListRepository : IBaseRepository<ToDoList>
{
    IQueryable<ToDoList> FindAllByGroup(int groupId, CancellationToken cancellationToken = default);
}