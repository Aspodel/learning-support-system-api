using LearningSupportSystemAPI.Core.Entities;

namespace LearningSupportSystemAPI.DataObjects
{
    public class CourseDTO : BaseDTO
    {
        public string CourseCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Credits { get; set; }
        public bool? GpaCalculated { get; set; } = true;

        public int DepartmentId { get; set; }
        public virtual ICollection<Major> Majors { get; set; } = new HashSet<Major>();
        public virtual ICollection<ClassDTO> Classes { get; set; } = new HashSet<ClassDTO>();
        public virtual ICollection<CoursePrerequisite> PrerequisiteFor { get; set; } = new HashSet<CoursePrerequisite>();
        public virtual ICollection<CoursePrerequisite> Prerequisites { get; set; } = new HashSet<CoursePrerequisite>();

    }
}
