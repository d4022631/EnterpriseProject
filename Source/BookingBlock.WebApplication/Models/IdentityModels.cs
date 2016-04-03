using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BookingBlock.WebApplication.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.

    [DbConfigurationType(typeof(ApplicationDbConfiguration))]
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<BusinessType> BusinessTypes { get; set; } 
    }

    public class ApplicationDbConfiguration : DbConfiguration
    {
        public ApplicationDbConfiguration()
        {
            this.SetDatabaseInitializer(new ApplicationDbInitializer());
        }
    }
}