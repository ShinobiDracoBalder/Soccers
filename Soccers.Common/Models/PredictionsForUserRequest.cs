using System;
using System.ComponentModel.DataAnnotations;

namespace Soccers.Common.Models
{
    public class PredictionsForUserRequest
    {
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public int TournamentId { get; set; }

        [Required]
        public string CultureInfo { get; set; }
    }
}
