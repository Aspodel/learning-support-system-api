using System.ComponentModel.DataAnnotations;

namespace LearningSupportSystemAPI;

public class LecturerDTO : UserDTO
{
    public string? Description { get; set; }
    public int? Salary { get; set; }

    public ICollection<ClassDTO>? Classes { get; set; }
    public ICollection<StudentDTO>? Students { get; set; }
}

public class CreateLecturerDTO
{
    [Required]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    public string LastName { get; set; } = string.Empty;

    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    public DateTime DateOfBirth { get; set; }

    public bool? Gender { get; set; }
    public string? Address { get; set; }
    public string? Avatar { get; set; }

    [Required]
    public int DepartmentId { get; set; }
}
public class CreateLecturerExcelDTO
{
    [Required]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    public string LastName { get; set; } = string.Empty;

    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    public DateTime DateOfBirth { get; set; }

    public bool? Gender { get; set; }
    public string? Address { get; set; }
    public string? Avatar { get; set; }

    [Required]
    public string Department { get; set; } = string.Empty;
}
