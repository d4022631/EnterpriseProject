using BookingBlock.EntityFramework;

namespace BookingBlock.WebApplication.Models
{
    public class IdentityServerUserService : AspNetIdentityUserService<ApplicationUser, string>
    {
        public IdentityServerUserService(ApplicationUserManager userManager) : base(userManager)
        {

        }
    }
}