using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Soccers.Common.Enums;
using Soccers.Web.Data;
using Soccers.Web.Data.Entities;
using Soccers.Web.Models;
using System;
using System.Threading.Tasks;

namespace Soccers.Web.Helpers
{
    public class UserHelper : IUserHelper
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<UserEntity> _signInManager;
        private readonly DataContext _dataContext;

        public UserHelper(UserManager<UserEntity> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<UserEntity> signInManager,
            DataContext dataContext){
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _dataContext = dataContext;
        }
        public async Task<IdentityResult> AddUserAsync(UserEntity user, string password){
            return await _userManager.CreateAsync(user, password);
        }
        public async Task AddUserToRoleAsync(UserEntity user, string roleName){
            await _userManager.AddToRoleAsync(user, roleName);
        }
        public async Task CheckRoleAsync(string roleName){
            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists){
                await _roleManager.CreateAsync(new IdentityRole{
                    Name = roleName
                });
            }
        }

        public async Task<UserEntity> GetUserAsync(string email)
        {
            return await _dataContext.Users
               .Include(u => u.Team)
               .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<UserEntity> GetUserAsync(Guid userId)
        {
            return await _dataContext.Users
                .Include(u => u.Team)
                .FirstOrDefaultAsync(u => u.Id == userId.ToString());
        }

        public async Task<UserEntity> GetUserByEmailAsync(string email){
            return await _userManager.FindByEmailAsync(email);
        }
        public async Task<bool> IsUserInRoleAsync(UserEntity user, string roleName){
            return await _userManager.IsInRoleAsync(user, roleName);
        }
        public async Task<SignInResult> LoginAsync(LoginViewModel model)
        {
            return await _signInManager.PasswordSignInAsync(
                model.Username,
                model.Password,
                model.RememberMe,
                false);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<UserEntity> AddUserAsync(AddUserViewModel model, string path, UserType userType)
        {
            UserEntity userEntity = new UserEntity
            {
                Address = model.Address,
                Document = model.Document,
                Email = model.Username,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PicturePath = path,
                PhoneNumber = model.PhoneNumber,
                Team = await _dataContext.Teams.FindAsync(model.TeamId),
                UserName = model.Username,
                UserType = userType
            };

            IdentityResult result = await _userManager.CreateAsync(userEntity, model.Password);
            if (result != IdentityResult.Success)
            {
                return null;
            }

            UserEntity newUser = await GetUserAsync(model.Username);
            await AddUserToRoleAsync(newUser, userEntity.UserType.ToString());
            return newUser;
        }
    }
}
