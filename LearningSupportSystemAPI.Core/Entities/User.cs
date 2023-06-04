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
        public DateTime? DateOfBirth { get; set; }
        public string? Address { get; set; }
        public string? Avatar { get; set; }
        public bool IsDeleted { get; set; } = false;

        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }

        public ICollection<UserRole>? UserRoles { get; set; }
        public ICollection<Announcement>? Announcements { get; set; }
        public ICollection<Discussion>? Discussions { get; set; } 
        public ICollection<Message>? Messages { get; set; }
        public ICollection<Notification>? Notifications { get; set; }
    }
}
