using Microsoft.AspNetCore.Http;
using Soccers.Web.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Soccers.Web.Models
{
    public class TeamViewModel : TeamEntity
    {
        [Display(Name = "Logo")]
        public IFormFile LogoFile { get; set; }
    }
}
