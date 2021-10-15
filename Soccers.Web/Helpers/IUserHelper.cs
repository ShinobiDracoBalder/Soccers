using Microsoft.AspNetCore.Identity;
using Soccers.Common.Enums;
using Soccers.Web.Data.Entities;
using Soccers.Web.Models;
using System;
using System.Threading.Tasks;

namespace Soccers.Web.Helpers
{
    public interface IUserHelper
    {
        Task<UserEntity> GetUserByEmailAsync(string email);
        Task<UserEntity> GetUserAsync(string email);
        Task<UserEntity> GetUserAsync(Guid userId);
        Task<IdentityResult> AddUserAsync(UserEntity user, string password);
        Task CheckRoleAsync(string roleName);
        Task AddUserToRoleAsync(UserEntity user, string roleName);
        Task<bool> IsUserInRoleAsync(UserEntity user, string roleName);
        Task<SignInResult> LoginAsync(LoginViewModel model);
        Task LogoutAsync();
        Task<UserEntity> AddUserAsync(AddUserViewModel model, string path, UserType userType);
    }
}
