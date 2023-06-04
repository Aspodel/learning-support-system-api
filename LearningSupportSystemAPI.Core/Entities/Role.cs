using Microsoft.AspNetCore.Identity;

namespace LearningSupportSystemAPI.Core.Entities
{
    public class Role : IdentityRole
    {
        public ICollection<UserRole>? UserRoles { get; }
    }
}
