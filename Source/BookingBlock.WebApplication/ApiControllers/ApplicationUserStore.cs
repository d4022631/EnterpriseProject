using System.Data.Entity;
using BookingBlock.EntityFramework;
using BookingBlock.WebApplication.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BookingBlock.WebApplication.ApiControllers
{
    public class ApplicationUserStore : UserStore<ApplicationUser>
    {
        public ApplicationUserStore(DbContext context) : base(context)
        {
           
        }
    }
}