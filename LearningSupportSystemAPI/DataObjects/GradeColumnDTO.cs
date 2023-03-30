using LearningSupportSystemAPI.Core.Entities;

namespace LearningSupportSystemAPI.DataObjects
{
    public class GradeColumnDTO : BaseDTO
    {
        public string Name { get; set; } = string.Empty;
        public bool? IsPublished { get; set; } = false;
        public int Percentage { get; set; }

        public int ClassId { get; set; }
        public virtual ICollection<Grade> Grades { get; set; } = new HashSet<Grade>();
    }
}
