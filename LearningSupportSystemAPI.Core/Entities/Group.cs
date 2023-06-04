using LearningSupportSystemAPI.Core.Entities.JoinTables;

namespace LearningSupportSystemAPI.Core.Entities
{
    public class Group : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string ProjectName { get; set; } = string.Empty;

        public ICollection<StudentClass> Students { get; set; } = new HashSet<StudentClass>();
    }
}
