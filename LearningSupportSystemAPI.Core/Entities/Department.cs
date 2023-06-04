namespace LearningSupportSystemAPI.Core.Entities
{
    public class Department : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string ShortName { get; set; } = string.Empty;
        public string? Description { get; set; }

        public int? FacultyOfficeId { get; set; }
        public Room? FacultyOffice { get; set; }

        public ICollection<User> Users { get; set; } = new HashSet<User>();
        public ICollection<Major> Majors { get; set; } = new HashSet<Major>();
        public ICollection<Course> Courses { get; set; } = new HashSet<Course>();
    }
}
