using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BookingBlock.EntityFramework
{
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

        public System.Data.Entity.DbSet<BusinessOpeningTime> BusinessOpeningTimes { get; set; }

        public System.Data.Entity.DbSet<Review> Reviews { get; set; }

        public DbSet<BookingLog> BookingLogs { get; set; }

    }
}