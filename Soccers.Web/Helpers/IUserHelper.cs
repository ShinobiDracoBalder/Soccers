using Microsoft.AspNetCore.Identity;
using Soccers.Web.Data.Entities;
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
    }
}
