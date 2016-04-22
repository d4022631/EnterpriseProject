using BookingBlock.EntityFramework;
using BookingBlock.WebApplication.Models;

namespace BookingBlock.WebApplication.ApiControllers
{
    public abstract class ApplicationDbContextStore
    {
        protected ApplicationDbContext _context;

        protected ApplicationDbContextStore(ApplicationDbContext context)
        {
            this._context = context;
        }
    }
}