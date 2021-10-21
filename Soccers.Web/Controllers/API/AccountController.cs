using Microsoft.AspNetCore.Mvc;
using Soccers.Web.Data;
using Soccers.Web.Helpers;

namespace Soccers.Web.Controllers.API
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _dataContext;
        private readonly IUserHelper _userHelper;
        private readonly IConverterHelper _converterHelper;

        public AccountController(DataContext dataContext,
            IUserHelper userHelper,
            IConverterHelper converterHelper)
        {
            _dataContext = dataContext;
            _userHelper = userHelper;
            _converterHelper = converterHelper;
        }
    }
}
