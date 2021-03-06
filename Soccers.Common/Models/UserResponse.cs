using Soccers.Common.Enums;

namespace Soccers.Common.Models
{
    public class UserResponse
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Document { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string PicturePath { get; set; }

        public LoginType LoginType { get; set; }

        public string PictureFullPath => string.IsNullOrEmpty(PicturePath)
            ? "https://SoccerWeb0.azurewebsites.net//images/noimage.png"
            : LoginType == LoginType.Soccer ? $"https://zulusoccer.blob.core.windows.net/users/{PicturePath}" : PicturePath;
        public string ImageFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(PicturePath))
                {
                    return "noimage";
                }

                return string.Format(
                    "http://socceronline.comtecom.com.mx:8085/{0}",
                    PicturePath.Substring(1));
            }
        }

        public UserType UserType { get; set; }

        public TeamResponse Team { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public string FullNameWithDocument => $"{FirstName} {LastName} - {Document}";
    }
}