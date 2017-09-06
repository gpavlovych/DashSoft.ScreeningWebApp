using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Screening.WebAPI.Core.Data;

namespace Screening.WebAPI.Core
{
    public class CombinedUserManager : UserManager<User>
    {
        public CombinedUserManager(
            IUserStore<User> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<User> passwordHasher,
            IEnumerable<IUserValidator<User>> userValidators,
            IEnumerable<IPasswordValidator<User>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<User>> logger) : base(store, optionsAccessor, passwordHasher, userValidators,
            passwordValidators, keyNormalizer, errors, services, logger)
        {
        }

        public override async Task<User> FindByNameAsync(string userName)
        {
            if (IsUserInFakeActiveDirectory(userName))
                return new User {UserName = userName, IsActiveDirectory = true};

            return await base.FindByNameAsync(userName);
        }

        public override async Task<IList<string>> GetRolesAsync(User user)
        {
            if (IsUserInFakeActiveDirectory(user.UserName))
                return new List<string> {"teacher"};

            return await base.GetRolesAsync(user);
        }

        public override async Task<bool> CheckPasswordAsync(User user, string password)
        {
            if (IsUserInFakeActiveDirectory(user.UserName))
                return password == "best!teacher";

            return await base.CheckPasswordAsync(user, password);
        }

        public override async Task<User> GetUserAsync(ClaimsPrincipal principal)
        {
            var user = await base.GetUserAsync(principal);
            if (user == null)
            {
                var claims = principal.Claims.ToDictionary(it => it.Type, it => it.Value);
                user = await FindByNameAsync(claims[ClaimTypes.NameIdentifier]);
            }

            return user;
        }

        private bool IsUserInFakeActiveDirectory(string userName)
        {
            if (userName == null)
                throw new ArgumentNullException(nameof(userName));

            return userName.StartsWith("SCHOOLDOMAIN\\");
        }
    }
}
