using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace BookingBlock.Identity
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
