using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Soccers.Common.Enums;
using Soccers.Common.Models;
using Soccers.Web.Data;
using Soccers.Web.Data.Entities;
using Soccers.Web.Helpers;
using Soccers.Web.Resources;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Soccers.Web.Controllers.API
{
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class AccountController : BaseApiController
    {
        private readonly DataContext _dataContext;
        private readonly IUserHelper _userHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IImageHelper _imageHelper;
        private readonly IMailHelper _mailHelper;

        public AccountController(DataContext dataContext,
            IUserHelper userHelper,
            IConverterHelper converterHelper,
            IImageHelper imageHelper,
            IMailHelper mailHelper)
        {
            _dataContext = dataContext;
            _userHelper = userHelper;
            _converterHelper = converterHelper;
            _imageHelper = imageHelper;
            _mailHelper = mailHelper;
        }
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(UserRequest))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostUser([FromBody] UserRequest request)
        {
            if (!ModelState.IsValid){
                return BadRequest(new Response{
                    IsSuccess = false,
                    Message = "Bad request",
                    Result = ModelState
                });
            }

            CultureInfo cultureInfo = new CultureInfo(request.CultureInfo);
            Resource.Culture = cultureInfo;

            UserEntity user = await _userHelper.GetUserAsync(request.Email);
            if (user != null){
                return BadRequest(new Response{
                    IsSuccess = false,
                    Message = Resource.UserAlreadyExists
                });
            }

            string picturePath = string.Empty;
            if (request.PictureArray != null && request.PictureArray.Length > 0){
                //picturePath = await _blobHelper.UploadBlobAsync(request.PictureArray, "users");
                picturePath = _imageHelper.UploadImage(request.PictureArray, "users");
            }

            user = new UserEntity{
                Address = request.Address,
                Document = request.Document,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.Phone,
                UserName = request.Email,
                PicturePath = picturePath,
                UserType = UserType.User,
                Team = await _dataContext.Teams.FindAsync(request.TeamId)
            };

            IdentityResult result = await _userHelper.AddUserAsync(user, request.Password);
            if (result != IdentityResult.Success){
                return BadRequest(result.Errors.FirstOrDefault().Description);
            }

            UserEntity userNew = await _userHelper.GetUserAsync(request.Email);
            await _userHelper.AddUserToRoleAsync(userNew, user.UserType.ToString());

            string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
            string tokenLink = Url.Action("ConfirmEmail", "Account", new{
                userid = user.Id,
                token = myToken
            }, protocol: HttpContext.Request.Scheme);

            _mailHelper.SendMail(request.Email, Resource.ConfirmEmail, $"<h1>{Resource.ConfirmEmail}</h1>" +
            $"{Resource.ConfirmEmailSubject}</br></br><a href = \"{tokenLink}\">{Resource.ConfirmEmail}</a>");

            return Ok(new Response
            {
                IsSuccess = true,
                Message = Resource.ConfirmEmailMessage
            });
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "Bad request",
                    Result = ModelState
                });
            }

            CultureInfo cultureInfo = new CultureInfo(request.CultureInfo);
            Resource.Culture = cultureInfo;

            UserEntity user = await _userHelper.GetUserAsync(request.Email);
            if (user == null)
            {
                return BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = Resource.UserDoesntExists
                });
            }

            IdentityResult result = await _userHelper.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
            if (!result.Succeeded)
            {
                var message = result.Errors.FirstOrDefault().Description;
                return BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = message.Contains("password") ? Resource.IncorrectCurrentPassword : message
                });
            }

            return Ok(new Response
            {
                IsSuccess = true,
                Message = Resource.PasswordChangedSuccess
            });
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        [Route("GetUserByEmail")]
        public async Task<IActionResult> GetUserByEmail([FromBody] EmailRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            CultureInfo cultureInfo = new CultureInfo(request.CultureInfo);
            Resource.Culture = cultureInfo;

            UserEntity userEntity = await _userHelper.GetUserAsync(request.Email);
            if (userEntity == null)
            {
                return NotFound(Resource.UserDoesntExists);
            }

            return Ok(_converterHelper.ToUserResponse(userEntity));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut]
        [ProducesResponseType(204, Type = typeof(UserRequest))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutUser([FromBody] UserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CultureInfo cultureInfo = new CultureInfo(request.CultureInfo);
            Resource.Culture = cultureInfo;

            UserEntity userEntity = await _userHelper.GetUserAsync(request.Email);
            if (userEntity == null)
            {
                return BadRequest(Resource.UserDoesntExists);
            }

            string picturePath = userEntity.PicturePath;
            if (request.PictureArray != null && request.PictureArray.Length > 0)
            {
                //picturePath = await _blobHelper.UploadBlobAsync(request.PictureArray, "users");
                picturePath = _imageHelper.UploadImage(request.PictureArray, "users");
            }

            userEntity.FirstName = request.FirstName;
            userEntity.LastName = request.LastName;
            userEntity.Address = request.Address;
            userEntity.PhoneNumber = request.Phone;
            userEntity.Document = request.Phone;
            userEntity.Team = await _dataContext.Teams.FindAsync(request.TeamId);
            userEntity.PicturePath = picturePath;

            IdentityResult respose = await _userHelper.UpdateUserAsync(userEntity);
            if (!respose.Succeeded)
            {
                return BadRequest(respose.Errors.FirstOrDefault().Description);
            }

            return NoContent();
        }

        [HttpPost]
        [Route("RecoverPassword")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EmailRequest))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RecoverPassword([FromBody] EmailRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "Bad request",
                    Result = ModelState
                });
            }

            CultureInfo cultureInfo = new CultureInfo(request.CultureInfo);
            Resource.Culture = cultureInfo;

            UserEntity user = await _userHelper.GetUserAsync(request.Email);
            if (user == null)
            {
                return BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = Resource.UserDoesntExists
                });
            }

            string myToken = await _userHelper.GeneratePasswordResetTokenAsync(user);
            string link = Url.Action("ResetPassword", "Account", new { token = myToken }, protocol: HttpContext.Request.Scheme);
           var resultEmail = _mailHelper.SendMail(request.Email, Resource.RecoverPassword, $"<h1>{Resource.RecoverPassword}</h1>" +
                $"{Resource.RecoverPasswordSubject}</br></br><a href = \"{link}\">{Resource.RecoverPassword}</a>");

            if (!resultEmail.IsSuccess)
            {
                return BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = resultEmail.Message,
                });
            }

            return Ok(new Response
            {
                IsSuccess = true,
                Message = Resource.RecoverPasswordMessage
            });
        }
    }
}
