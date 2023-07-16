using System.ComponentModel.DataAnnotations;

namespace LearningSupportSystemAPI;

public class UserDTO : BaseDTO<string>
{
    public string IdCard { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? FullName { get; set; } = string.Empty;
    public bool? Gender { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Avatar { get; set; }
    public int? DepartmentId { get; set; }
    public ICollection<string> Roles { get; set; } = new HashSet<string>();
    public ICollection<AnnouncementDTO> Announcements { get; set; } = new HashSet<AnnouncementDTO>();
    public ICollection<DiscussionDTO> Discussions { get; set; } = new HashSet<DiscussionDTO>();
    public ICollection<MessageDTO> Messages { get; set; } = new HashSet<MessageDTO>();
    public ICollection<NotificationDTO> Notifications { get; set; } = new HashSet<NotificationDTO>();

}

public class CreateUserDTO
{
    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    [Required]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    public string LastName { get; set; } = string.Empty;

    [EmailAddress]
    public string? Email { get; set; } = string.Empty;

    public int? DepartmentId { get; set; }

    public IList<string> Roles { get; set; } = Array.Empty<string>();
}
