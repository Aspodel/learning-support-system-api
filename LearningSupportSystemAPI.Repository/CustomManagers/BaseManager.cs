using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq.Expressions;

namespace LearningSupportSystemAPI;

public abstract class BaseManager<TUser> : UserManager<TUser> where TUser : User
{
    public BaseManager(
        IUserStore<TUser> store,
        IOptions<IdentityOptions> optionsAccessor,
        IPasswordHasher<TUser> passwordHasher,
        IEnumerable<IUserValidator<TUser>> userValidators,
        IEnumerable<IPasswordValidator<TUser>> passwordValidators,
        ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors,
        IServiceProvider services,
        ILogger<UserManager<TUser>> logger
    ) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
    {
    }

    public virtual async Task<TUser?> FindByIdCardAsync(string idCard)
        => await Users.Where(u => u.IdCard == idCard).FirstOrDefaultAsync();

    public new async Task<TUser?> FindByNameAsync(string userName)
    {
        var user = await base.FindByNameAsync(userName);
        if (user is null || user.IsDeleted)
            return null;

        return user;
    }

    public virtual IQueryable<TUser> FindAll(Expression<Func<TUser, bool>>? predicate = null)
        => Users
            .Where(u => !u.IsDeleted)
            .WhereIf(predicate != null, predicate!);
}
