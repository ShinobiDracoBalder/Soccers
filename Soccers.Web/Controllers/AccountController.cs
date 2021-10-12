using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Soccers.Web.Data;
using System.Threading.Tasks;

namespace Soccers.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly DataContext _dataContext;

        public AccountController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _dataContext.Users
        //        .Include(u => u.Team)
        //        .Include(u => u.Predictions)
        //        .Where(u => u.UserType == UserType.User)
        //        .OrderBy(u => u.FirstName)
        //        .ThenBy(u => u.LastName)
        //        .ToListAsync());
        //}

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
    }
}
