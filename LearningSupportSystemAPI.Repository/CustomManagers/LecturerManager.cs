using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LearningSupportSystemAPI;

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

    public override async Task<Lecturer?> FindByIdCardAsync(string idCard)
        => await Users
                    .Where(u => u.IdCard == idCard)
                    .Include(u => u.Classes)
                    .FirstOrDefaultAsync();

    public IQueryable<Lecturer> FindAllByDepartment(int departmentId, Expression<Func<Lecturer, bool>>? predicate = null)
        => Users
            .WhereIf(predicate != null, predicate!)
            .Where(u => u.DepartmentId == departmentId);
}
