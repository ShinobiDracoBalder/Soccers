using System.ComponentModel.DataAnnotations;

namespace Soccers.Web.Models
{
    public class RecoverPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
