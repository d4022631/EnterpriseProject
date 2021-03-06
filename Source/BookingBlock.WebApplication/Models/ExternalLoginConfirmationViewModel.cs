﻿using System.ComponentModel.DataAnnotations;

namespace BookingBlock.WebApplication.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
