using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Screening.WebAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace Screening.WebAPI
{
    public class FakeApplicationUserStore : IUserPasswordStore<ApplicationUser>, IUserStore<ApplicationUser>, IUserEmailStore<ApplicationUser>
    {
        private static Dictionary<string, ApplicationUser> users = new Dictionary<string, ApplicationUser>();

        public async Task CreateAsync(ApplicationUser user)
        {
            users.Add(user.Id, user);
        }

        public async Task DeleteAsync(ApplicationUser user)
        {
            users.Remove(user.Id);
        }

        public void Dispose()
        { 
        }

        public async Task<ApplicationUser> FindByEmailAsync(string email)
        {
            return users.Values.FirstOrDefault(usr => usr.Email == email);
        }

        public async Task<ApplicationUser> FindByIdAsync(string userId)
        {
            return users[userId];
        }

        public async Task<ApplicationUser> FindByNameAsync(string userName)
        {
            return users.Values.FirstOrDefault(usr=>usr.UserName == userName);
        }

        public async Task<string> GetEmailAsync(ApplicationUser user)
        {
            return user.Email;
        }

        public async Task<bool> GetEmailConfirmedAsync(ApplicationUser user)
        {
            return user.EmailConfirmed;
        }

        public async Task<string> GetPasswordHashAsync(ApplicationUser user)
        {
            return user.PasswordHash;
        }

        public async Task<bool> HasPasswordAsync(ApplicationUser user)
        {
            return user.PasswordHash != null;
        }

        public async Task SetEmailAsync(ApplicationUser user, string email)
        {
            user.Email = email;
        }

        public async Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed)
        {
            user.EmailConfirmed = confirmed;
        }

        public async Task SetPasswordHashAsync(ApplicationUser user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
        }

        public async Task UpdateAsync(ApplicationUser user)
        {
            users[user.Id] = user;
        }
    }
}