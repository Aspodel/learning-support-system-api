using LearningSupportSystemAPI.Core.Entities;

namespace LearningSupportSystemAPI.DataObjects
{
    public class MajorDTO : BaseDTO
    {
        public string Name { get; set; } = string.Empty;
        public string ShortName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int? FeePerCredit { get; set; }

        public int DepartmentId { get; set; }
        public ICollection<CourseDTO> CourseProgram { get; set; } = new HashSet<CourseDTO>();
        public ICollection<StudentDTO> Students { get; set; } = new HashSet<StudentDTO>();
    }

    public class CreateMajorDTO
    {
        public string Name { get; set; } = string.Empty;
        public string ShortName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int? FeePerCredit { get; set; }

        public int DepartmentId { get; set; }
    }
}
