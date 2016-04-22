using System.ComponentModel.DataAnnotations;

namespace BookingBlock.WebApplication.Models
{
    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}