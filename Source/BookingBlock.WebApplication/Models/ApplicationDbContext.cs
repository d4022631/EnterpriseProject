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
            this.Configuration.LazyLoadingEnabled = false;
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<BusinessType> BusinessTypes { get; set; } 

        public DbSet<Booking> Bookings { get; set; }
        
        public DbSet<Business> Businesses { get; set; }
        
        public DbSet<Service> Services { get; set; }

        public DbSet<BusinessUser> BusinessUsers { get; set; }

        public System.Data.Entity.DbSet<BookingBlock.WebApplication.Models.BusinessOpeningTime> BusinessOpeningTimes { get; set; }

        public System.Data.Entity.DbSet<BookingBlock.WebApplication.Models.Review> Reviews { get; set; }
    }
}