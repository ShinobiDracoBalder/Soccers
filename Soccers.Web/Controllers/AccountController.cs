using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Soccers.Common.Enums;
using Soccers.Web.Data;
using Soccers.Web.Data.Entities;
using Soccers.Web.Helpers;
using Soccers.Web.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Soccers.Web.Controllers
{

    public class AccountController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IUserHelper _userHelper;
        private readonly IImageHelper _imageHelper;
        private readonly ICombosHelper _combosHelper;
        private readonly IConfiguration _configuration;
        private readonly IMailHelper _mailHelper;

        public AccountController(DataContext dataContext, IUserHelper userHelper, 
            IImageHelper imageHelper,
            ICombosHelper combosHelper,
            IConfiguration configuration, IMailHelper mailHelper){
            _dataContext = dataContext;
            _userHelper = userHelper;
            _imageHelper = imageHelper;
            _combosHelper = combosHelper;
            _configuration = configuration;
            _mailHelper = mailHelper;
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index(){
            return View(await _dataContext.Users
                .Include(u => u.Team)
                .Include(u => u.Predictions)
                .Where(u => u.UserType == UserType.User)
                .OrderBy(u => u.FirstName)
                .ThenBy(u => u.LastName)
                .ToListAsync());
        }
        public async Task<IActionResult> Details(string id){
            if (id == null)
            {
                return new NotFoundViewResult("_ResourceNotFound");
            }

            Data.Entities.UserEntity _user = await _dataContext.Users
                .Include(u => u.Team)
                .Include(u => u.Predictions)
                .Where(u => u.UserType == UserType.User && u.Id.Equals(id))
                .FirstOrDefaultAsync();

            if (_user == null)
            {
                return new NotFoundViewResult("_ResourceNotFound");
            }

            return View(_user);
        }
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _userHelper.LoginAsync(model);
                if (result.Succeeded)
                {
                    if (Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return Redirect(Request.Query["ReturnUrl"].First());
                    }

                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError(string.Empty, "Failed to login.");
            return View(model);
        }
        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }
        public IActionResult NotAuthorized()
        {
            return View();
        }
        public IActionResult Register()
        {
            AddUserViewModel model = new AddUserViewModel
            {
                Teams = _combosHelper.GetComboTeams()
            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(AddUserViewModel model)
        {

            if (ModelState.IsValid)
            {
                System.Threading.Thread.Sleep(30);
                string path = string.Empty;

                if (model.PictureFile != null){
                    //path = await _blobHelper.UploadBlobAsync(model.PictureFile, "users");
                    path = await _imageHelper.UploadImageAsync(model.PictureFile, "users");
                }

                UserEntity user = await _userHelper.AddUserAsync(model, path, UserType.User);
                if (user == null){
                    ModelState.AddModelError(string.Empty, "This email is already used.");
                    model.Teams = _combosHelper.GetComboTeams();
                    return View(model);
                }
                //    LoginViewModel loginViewModel = new LoginViewModel{
                //        Password = model.Password,
                //         RememberMe = false,
                //         Username = model.Username
                //    };
                //var result2 = await _userHelper.LoginAsync(loginViewModel);
                //if (result2.Succeeded){
                //    return RedirectToAction("Index", "Home");
                //}
                var myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                var tokenLink = Url.Action("ConfirmEmail", "Account", new
                {
                    userid = user.Id,
                    token = myToken
                }, protocol: HttpContext.Request.Scheme);

                var response = _mailHelper.SendMail(model.Username, "Email confirmation", $"<h1>Email Confirmation</h1>" +
                    $"To allow the user, " +
                    $"plase click in this link:</br></br><a href = \"{tokenLink}\">Confirm Email</a>");
                if (response.IsSuccess)
                {
                    ViewBag.Message = "The instructions to allow your user has been sent to email.";
                    return View(model);
                }

                ModelState.AddModelError(string.Empty, response.Message);
            }

            model.Teams = _combosHelper.GetComboTeams();
            return View(model);
        }
        public async Task<IActionResult> ChangeUser()
        {
            UserEntity user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }

            EditUserViewModel model = new EditUserViewModel{
                Address = user.Address,
                Document = user.Document,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                PicturePath = user.ImageFullPath,
                Teams = _combosHelper.GetComboTeams(),
                TeamId = user.Team.Id
            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeUser(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                string path = model.PicturePath;

                if (model.PictureFile != null)
                {
                    //path = await _blobHelper.UploadBlobAsync(model.PictureFile, "users");
                    path = await _imageHelper.UploadImageAsync(model.PictureFile, "users");
                }

                UserEntity user = await _userHelper.GetUserAsync(User.Identity.Name);

                user.Document = model.Document;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Address = model.Address;
                user.PhoneNumber = model.PhoneNumber;
                user.PicturePath = path;
                user.Team = await _dataContext.Teams.FindAsync(model.TeamId);

                await _userHelper.UpdateUserAsync(user);
                return RedirectToAction("Index", "Home");
            }

            model.Teams = _combosHelper.GetComboTeams();
            return View(model);
        }
        public IActionResult ChangerPasswordMVC()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChangerPasswordMVC(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                UserEntity user = await _userHelper.GetUserAsync(User.Identity.Name);
                if (user != null)
                {
                    Microsoft.AspNetCore.Identity.IdentityResult result = await _userHelper.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ChangeUser");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User no found.");
                }
            }

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateToken([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                UserEntity user = await _userHelper.GetUserAsync(model.Username);
                if (user != null)
                {
                    Microsoft.AspNetCore.Identity.SignInResult result = await _userHelper.ValidatePasswordAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        object results = GetToken(user.Email);
                        return Created(string.Empty, results);
                    }
                }
            }

            return BadRequest();
        }
        private object GetToken(string email)
        {
            Claim[] claims = new[]{
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken token = new JwtSecurityToken(
                _configuration["Tokens:Issuer"],
                _configuration["Tokens:Audience"],
                claims,
                expires: DateTime.UtcNow.AddDays(15),
                signingCredentials: credentials);
            return new{
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            };
        }
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return NotFound();
            }

            UserEntity user = await _userHelper.GetUserAsync(new Guid(userId));
            if (user == null)
            {
                return NotFound();
            }

            IdentityResult result = await _userHelper.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return NotFound();
            }

            return View();
        }
        public IActionResult RecoverPasswordMVC()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RecoverPasswordMVC(RecoverPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                UserEntity user = await _userHelper.GetUserAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "The email doesn't correspont to a registered user.");
                    return View(model);
                }

                string myToken = await _userHelper.GeneratePasswordResetTokenAsync(user);
                string link = Url.Action(
                    "ResetPassword",
                    "Account",
                    new { token = myToken }, protocol: HttpContext.Request.Scheme);
                _mailHelper.SendMail(model.Email, "Soccer Password Reset", $"<h1>Soccer Password Reset</h1>" +
                    $"To reset the password click in this link:</br></br>" +
                    $"<a href = \"{link}\">Reset Password</a>");
                ViewBag.Message = "The instructions to recover your password has been sent to email.";
                return View();

            }

            return View(model);
        }

        public IActionResult ResetPassword(string token)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            UserEntity user = await _userHelper.GetUserAsync(model.UserName);
            if (user != null)
            {
                IdentityResult result = await _userHelper.ResetPasswordAsync(user, model.Token, model.Password);
                if (result.Succeeded)
                {
                    ViewBag.Message = "Password reset successful.";
                    return View();
                }

                ViewBag.Message = "Error while resetting the password.";
                return View(model);
            }

            ViewBag.Message = "User not found.";
            return View(model);
        }
    }
}
