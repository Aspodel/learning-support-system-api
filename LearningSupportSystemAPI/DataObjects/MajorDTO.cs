using LearningSupportSystemAPI.Core.Entities;

namespace LearningSupportSystemAPI.DataObjects
{
    public class MajorDTO : BaseDTO
    {
        public string Name { get; set; } = string.Empty;
        public string ShortName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int? FeePerCredit { get; set; } = 58;

        public int DepartmentId { get; set; }
        public virtual ICollection<Course> CourseProgram { get; set; } = new HashSet<Course>();
        public virtual ICollection<Student> Students { get; set; } = new HashSet<Student>();

    }
}
