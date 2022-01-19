using Microsoft.AspNetCore.Identity;
using MoneyManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MoneyManager.Services
{
    public class UserStore : IUserStore<User>, IUserEmailStore<User>, IUserPasswordStore<User>
    {
        private readonly IUserRepository userRepository;

        public UserStore(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            user.Id = await userRepository.Create(user);
            return IdentityResult.Success;
        }

        public Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
        }

        public async Task<User> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            return await userRepository.SearchUser(normalizedEmail);
        }

        public Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            return await userRepository.SearchUser(normalizedUserName);
        }

        public Task<string> GetEmailAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetNormalizedEmailAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHsh);
        }

        public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetEmailAsync(User user, string email, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetEmailConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedEmailAsync(User user, string normalizedEmail, CancellationToken cancellationToken)
        {
            user.EmailNormalize = normalizedEmail;
            return Task.CompletedTask;
        }

        public Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHsh = passwordHash;
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
