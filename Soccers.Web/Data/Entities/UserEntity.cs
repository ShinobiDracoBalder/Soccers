using Microsoft.AspNetCore.Identity;
using Soccers.Common.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Soccers.Web.Data.Entities
{
    public class UserEntity : IdentityUser
    {
        [Display(Name = "Document")]
        [MaxLength(20, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public string Document { get; set; }

        [Display(Name = "First Name")]
        [MaxLength(50, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [MaxLength(50, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public string LastName { get; set; }

        [MaxLength(500, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        public string Address { get; set; }

        [Display(Name = "Picture")]
        public string PicturePath { get; set; }

        [Display(Name = "Picture")]
        public string ImagePath { get; set; }

        [Display(Name = "User Type")]
        public UserType UserType { get; set; }

        [Display(Name = "Login Type")]
        public LoginType LoginType { get; set; }

        [Display(Name = "Favorite Team")]
        public TeamEntity Team { get; set; }

        public int Points => Predictions == null ? 0 : Predictions.Sum(p => p.Points);

        public string FullName => $"{FirstName} {LastName}";

        public string FullNameWithDocument => $"{FirstName} {LastName} - {Document}";

        [Display(Name = "Picture")]
        public string PictureFullPath => string.IsNullOrEmpty(PicturePath)
            ? "https://SoccerWeb0.azurewebsites.net//images/noimage.png"
            : LoginType == LoginType.Soccer ? $"https://zulusoccer.blob.core.windows.net/users/{PicturePath}" : PicturePath;


        public string ImageFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(ImagePath))
                {
                    //return "https://localhost:44372//images/noimage.png";
                    return "http://socceronline.comtecom.com.mx:8085/images/noimage.png";
                }

                //return string.Format(
                //    "https://localhost:44372/{0}",
                //    ImagePath.Substring(1));

                return string.Format(
                    "http://socceronline.comtecom.com.mx:8085{0}",
                    ImagePath.Substring(1));
            }
        }
       public ICollection<PredictionEntity> Predictions { get; set; }
    }
}
