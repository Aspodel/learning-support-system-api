namespace LearningSupportSystemAPI;

public interface IToDoItemRepository : IBaseRepository<ToDoItem>
{
    IQueryable<ToDoItem> FindAllByToDoList(int toDoListId, CancellationToken cancellationToken = default);
}