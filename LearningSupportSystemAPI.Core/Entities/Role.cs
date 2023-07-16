using Microsoft.AspNetCore.Identity;

namespace LearningSupportSystemAPI;

public class Role : IdentityRole
{
    public ICollection<UserRole>? UserRoles { get; }
}
