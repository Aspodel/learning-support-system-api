using Microsoft.AspNetCore.Identity;

namespace LearningSupportSystemAPI.Core.Entities
{
    public class Role : IdentityRole
    {
        public virtual ICollection<UserRole> UserRoles { get; } = new HashSet<UserRole>();
    }
}
