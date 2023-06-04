namespace LearningSupportSystemAPI.Core.Entities
{
    public class Course : BaseEntity
    {
        public string CourseCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int? Credits { get; set; }
        public bool? GpaCalculated { get; set; } = true;

        public int DepartmentId { get; set; }
        public Department? Department { get; set; }

        public ICollection<Major>? Majors { get; set; }
        public ICollection<Class> Classes { get; set; } = new HashSet<Class>();

        // Navigation property for the courses that this course is a prerequisite for
        public ICollection<CoursePrerequisite>? PrerequisiteFor { get; set; }

        // Navigation property for the courses that are prerequisites for this course
        public ICollection<CoursePrerequisite>? Prerequisites { get; set; }
    }
}
