using System.ComponentModel.DataAnnotations;

namespace LearningSupportSystemAPI;

public class StudentDTO : UserDTO
{
    public int? StartYear { get; set; }
    public int? MajorId { get; set; }
    public string? SupervisorId { get; set; }

    public ICollection<int> RegisteredClasses { get; set; } = new HashSet<int>();
    public ICollection<GradeDTO>? Grades { get; set; }
    public ICollection<StudentTaskDTO>? Tasks { get; set; }
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
}

public class RegisterCoursesDTO
{
    [Required]
    public string IdCard { get; set; } = string.Empty;
    [Required]
    public ICollection<int> RegisteredClasses { get; set; } = new HashSet<int>();
}

public class CreateStudentExcelDTO
{
    [Required]
    public string FirstName { get; set; } = string.Empty;
    [Required]
    public string LastName { get; set; } = string.Empty;
    [Required]
    public DateTime DateOfBirth { get; set; }
    [Required]
    public string Department { get; set; } = string.Empty;
    public int? MajorId { get; set; }
    public string? SupervisorId { get; set; }

    public int? StartYear { get; set; } = DateTime.UtcNow.Year;
    public bool? Gender { get; set; }
    public string? Address { get; set; }
    public string? Avatar { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
}

public class ExportStudentDTO
{
    public string IdCard { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? DateOfBirth { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
}
