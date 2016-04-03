using System.ComponentModel.DataAnnotations;
using BookingBlock.WebApplication.Models.ValidationAttributes;

namespace BookingBlock.WebApplication.ApiControllers
{
    public class RegisterAccountRequest
    {
        // first name
        // last name
        // email address
        // password
        // confirm password
        // date of birth
        // mobile number
        // address line 1
        // address line 2
        // town / city
        // postcode
        // country
        // gender

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }


        [BookingBlockPasswordValidator, Required]
        public string Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "The password and confirmation password do not match.")]
        [Required]
        public string ConfirmPassword { get; set; }


        [Required]
        public string DateOfBirth { get; set; }

        [Required]
        [Phone]
        public string MobileNumber { get; set; }

        [Required]
        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        [Required]
        public string TownCity { get; set; }

        [PostcodeValidator, Required]
        public string Postcode { get; set; }

        [Required]
        public string Country { get; set; }
        [Required]
        public string Gender { get; set; }
    }
}