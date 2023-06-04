﻿using LearningSupportSystemAPI.Core.Entities;
using LearningSupportSystemAPI.Repository.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LearningSupportSystemAPI.Repository
{
    public class StudentManager : BaseManager<Student>
    {
        public StudentManager(
            IUserStore<Student> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<Student> passwordHasher,
            IEnumerable<IUserValidator<Student>> userValidators,
            IEnumerable<IPasswordValidator<Student>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<Student>> logger
        ) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }

        public override async Task<Student?> FindByIdCardAsync(string idCard)
            => await Users
                        .Where(u => u.IdCard == idCard)
                        .Include(u => u.RegisteredClasses)
                            .ThenInclude(sc => sc.Class)
                        .FirstOrDefaultAsync();
    }
}
