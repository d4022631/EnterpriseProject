using System;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BookingBlock.EntityFramework
{
    public class ApplicationDbConfiguration : DbConfiguration
    {
        public ApplicationDbConfiguration()
        {
            this.SetDatabaseInitializer(new ApplicationDbInitializer());
        }
    }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

    }

}