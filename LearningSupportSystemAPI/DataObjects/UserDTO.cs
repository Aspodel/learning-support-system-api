using LearningSupportSystemAPI.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace LearningSupportSystemAPI.DataObjects
{
    public class UserDTO
    {
        public string IdCard { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public bool? Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Address { get; set; }
        public string? Avatar { get; set; }
        public int? DepartmentId { get; set; }
        public virtual ICollection<string> Roles { get; set; } = new HashSet<string>();
        public virtual ICollection<AnnouncementDTO> Announcements { get; set; } = new HashSet<AnnouncementDTO>();
        public virtual ICollection<DiscussionDTO> Discussions { get; set; } = new HashSet<DiscussionDTO>();
        public virtual ICollection<MessageDTO> Messages { get; set; } = new HashSet<MessageDTO>();
        public virtual ICollection<NotificationDTO> Notifications { get; set; } = new HashSet<NotificationDTO>();

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

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public IList<string> Roles { get; set; } = Array.Empty<string>();
    }
}
