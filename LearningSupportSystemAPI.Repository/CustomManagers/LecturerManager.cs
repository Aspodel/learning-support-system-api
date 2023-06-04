using LearningSupportSystemAPI.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LearningSupportSystemAPI.Repository
{
    public class LecturerManager : BaseManager<Lecturer>
    {
        public LecturerManager(
            IUserStore<Lecturer> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<Lecturer> passwordHasher,
            IEnumerable<IUserValidator<Lecturer>> userValidators,
            IEnumerable<IPasswordValidator<Lecturer>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<Lecturer>> logger
        ) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }
    }
}
