using LearningSupportSystemAPI.Core.Entities.JoinTables;

namespace LearningSupportSystemAPI.DataObjects
{
    public class GroupDTO : BaseDTO
    {
        public string Name { get; set; } = string.Empty;
        public string ProjectName { get; set; } = string.Empty;

        public virtual ICollection<StudentClass> Students { get; set; } = new HashSet<StudentClass>();

    }
}
