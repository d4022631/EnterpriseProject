using Microsoft.AspNet.Identity;

namespace BookingBlock.WebApplication
{
    /// <summary>
    /// Used to validate user password against the Booking Block password policy.
    /// </summary>
    public class BookingBlockPasswordValidator : PasswordValidator
    {
        public BookingBlockPasswordValidator()
        {
            RequiredLength = 6;
            RequireNonLetterOrDigit = true;
            RequireDigit = true;
            RequireLowercase = true;
            RequireUppercase = true;
        }
    }
}