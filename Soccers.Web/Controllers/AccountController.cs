using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Soccers.Common.Enums;
using Soccers.Web.Data;
using Soccers.Web.Data.Entities;
using Soccers.Web.Helpers;
using Soccers.Web.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Soccers.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IUserHelper _userHelper;
        private readonly IImageHelper _imageHelper;
        private readonly ICombosHelper _combosHelper;

        public AccountController(DataContext dataContext, IUserHelper userHelper, IImageHelper imageHelper,
            ICombosHelper combosHelper)
        {
            _dataContext = dataContext;
            _userHelper = userHelper;
            _imageHelper = imageHelper;
            _combosHelper = combosHelper;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.Users
                .Include(u => u.Team)
                .Include(u => u.Predictions)
                .Where(u => u.UserType == UserType.User)
                .OrderBy(u => u.FirstName)
                .ThenBy(u => u.LastName)
                .ToListAsync());
        }

        public async Task<IActionResult> Details(string id)
        {
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
        public IActionResult Register()
        {
            AddUserViewModel model = new AddUserViewModel
            {
                Teams = _combosHelper.GetComboTeams()
            };

            return View(model);
        }

        public IActionResult NotAuthorized()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(AddUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                string path = string.Empty;

                if (model.PictureFile != null)
                {
                    //path = await _blobHelper.UploadBlobAsync(model.PictureFile, "users");
                    path = await _imageHelper.UploadImageAsync(model.PictureFile, "users");
                }

                UserEntity user = await _userHelper.AddUserAsync(model, path, UserType.User);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "This email is already used.");
                    model.Teams = _combosHelper.GetComboTeams();
                    return View(model);
                }
                LoginViewModel loginViewModel = new LoginViewModel{
                    Password = model.Password,
                     RememberMe = false,
                     Username = model.Username
                };
            var result2 = await _userHelper.LoginAsync(loginViewModel);
            if (result2.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            //var myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
            //var tokenLink = Url.Action("ConfirmEmail", "Account", new
            //{
            //    userid = user.Id,
            //    token = myToken
            //}, protocol: HttpContext.Request.Scheme);

            //var response = _mailHelper.SendMail(model.Username, "Email confirmation", $"<h1>Email Confirmation</h1>" +
            //    $"To allow the user, " +
            //    $"plase click in this link:</br></br><a href = \"{tokenLink}\">Confirm Email</a>");
            //if (response.IsSuccess)
            //{
            //    ViewBag.Message = "The instructions to allow your user has been sent to email.";
            //    return View(model);
            //}

            //ModelState.AddModelError(string.Empty, response.Message);
        }

            model.Teams = _combosHelper.GetComboTeams();
            return View(model);
        }
    }
}
