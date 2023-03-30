using LearningSupportSystemAPI.Core.Entities;

namespace LearningSupportSystemAPI.DataObjects
{
    public class DepartmentDTO : BaseDTO
    {
        public string Name { get; set; } = string.Empty;
        public string ShortName { get; set; } = string.Empty;
        public string? Description { get; set; }

        public int? FacultyOfficeId { get; set; }
        public virtual ICollection<User> Users { get; set; } = new HashSet<User>();
        public virtual ICollection<Major> Majors { get; set; } = new HashSet<Major>();
        public virtual ICollection<Course> Courses { get; set; } = new HashSet<Course>();
    }
}
