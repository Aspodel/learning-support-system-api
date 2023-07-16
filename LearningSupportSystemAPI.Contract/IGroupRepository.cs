namespace LearningSupportSystemAPI;
public interface IGroupRepository : IBaseRepository<Group>
{
    IQueryable<Group> FindAllByClass(int classId);
}
