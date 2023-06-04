using LearningSupportSystemAPI.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LearningSupportSystemAPI.Repository
{
    public class AdminManager : BaseManager<Administrator>
    {
        public AdminManager(
            IUserStore<Administrator> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<Administrator> passwordHasher,
            IEnumerable<IUserValidator<Administrator>> userValidators,
            IEnumerable<IPasswordValidator<Administrator>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<Administrator>> logger
        ) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }
    }
}
