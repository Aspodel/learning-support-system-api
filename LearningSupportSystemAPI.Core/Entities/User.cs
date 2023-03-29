using Microsoft.AspNetCore.Identity;

namespace LearningSupportSystemAPI.Core.Entities
{
    public class User : IdentityUser
    {
        public string IdCard { get; set; } = null!;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}";
        public bool? Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Address { get; set; }
        public string? Avatar { get; set; }
        public bool? IsDeleted { get; set; } = false;

        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();
        public virtual ICollection<Announcement> Announcements { get; set; } = new HashSet<Announcement>();
        public virtual ICollection<Discussion> Discussions { get; set; } = new HashSet<Discussion>();
        public virtual ICollection<Message> Messages { get; set; } = new HashSet<Message>();
        public virtual ICollection<Notification> Notifications { get; set; } = new HashSet<Notification>();
    }
}
