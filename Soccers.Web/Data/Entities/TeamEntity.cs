using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Soccers.Web.Data.Entities
{
    public class TeamEntity
    {
        public int Id { get; set; }
        [MaxLength(100, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public string Name { get; set; }
        [Display(Name = "Logo")]
        public string LogoPath { get; set; }
        [Display(Name = "Logo")]
        public string LogoFullPath => string.IsNullOrEmpty(LogoPath)
           ? "https://localhost:44372//images/noimage.png"
           : $"https://zulusoccer.blob.core.windows.net/teams/{LogoPath}";
        [Display(Name = "Image")]
        public string ImageFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(LogoPath))
                {
                    return "https://localhost:44372//images/noimage.png";
                }

                return string.Format(
                    "https://localhost:44372/{0}",
                    LogoPath.Substring(1));
            }
        }
        public ICollection<GroupDetailEntity> GroupDetails { get; set; }
    }
}
