using System.ComponentModel.DataAnnotations;

namespace LearningSupportSystemAPI.DataObjects
{
    public class StudentDTO : UserDTO
    {
        public int? StartYear { get; set; }
        public int? MajorId { get; set; }
        public string? SupervisorId { get; set; }

        public ICollection<int> RegisteredClasses { get; set; } = new HashSet<int>();
        public ICollection<GradeDTO>? Grades { get; set; }
    }

    public class CreateStudentDTO
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [EmailAddress]
        public string? Email { get; set; }


        [Required]
        public DateTime DateOfBirth { get; set; }

        public int? StartYear { get; set; } = DateTime.UtcNow.Year;
        public bool? Gender { get; set; }
        public string? Address { get; set; }
        public string? Avatar { get; set; }

        [Required]
        public int DepartmentId { get; set; }
        public int? MajorId { get; set; }
        public string? SupervisorId { get; set; }
        public IList<string> Roles { get; set; } = Array.Empty<string>();
    }

    public class RegisterCoursesDTO
    {
        [Required]
        public string IdCard { get; set; } = string.Empty;
        [Required]
        public ICollection<int> RegisteredClasses { get; set; } = new HashSet<int>();
    }
}
