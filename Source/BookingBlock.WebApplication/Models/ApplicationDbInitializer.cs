using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BookingBlock.WebApplication.Models
{
    public class ApplicationDbInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            UserStore<ApplicationUser> userStore = new UserStore<ApplicationUser>(context);
            ApplicationUserManager applicationUserManager = new ApplicationUserManager(userStore);

            RoleStore<IdentityRole> roleStore = new RoleStore<IdentityRole>(context);

            RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(roleStore);

            var webmaster = new ApplicationUser();

            webmaster.Title = "Dr.";
            webmaster.FirstName = "bookingblock";
            webmaster.LastName = "webmaster";

            webmaster.Email = "webmaster@bookingblock.azurewebsites.net";
            webmaster.UserName = "webmaster@bookingblock.azurewebsites.net";

            var result = applicationUserManager.CreateAsync(webmaster, "Enterprise2016!")
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();

            var roleCreateResult =
                roleManager.CreateAsync(new IdentityRole("webmaster"))
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();

            var addToRoleResult =
                applicationUserManager.AddToRoleAsync(webmaster.Id, "webmaster")
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();

            base.Seed(context);
        }
    }
}