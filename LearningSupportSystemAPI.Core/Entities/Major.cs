namespace LearningSupportSystemAPI.Core.Entities
{
    public class Major : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string ShortName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int? FeePerCredit { get; set; } = 58;

        public int DepartmentId { get; set; }
        public Department? Department { get; set; }

        public ICollection<Course> CourseProgram { get; set; } = new HashSet<Course>();
        public ICollection<Student> Students { get; set; } = new HashSet<Student>();
    }
}
