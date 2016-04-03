using System.Data.Entity;

namespace BookingBlock.WebApplication.Models
{
    public class ApplicationDbConfiguration : DbConfiguration
    {
        public ApplicationDbConfiguration()
        {
            this.SetDatabaseInitializer(new ApplicationDbInitializer());
        }
    }
}