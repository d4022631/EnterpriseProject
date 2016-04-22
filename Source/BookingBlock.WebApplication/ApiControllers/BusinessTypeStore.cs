using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using BookingBlock.EntityFramework;
using BookingBlock.WebApplication.Models;

namespace BookingBlock.WebApplication.ApiControllers
{
    public sealed class BusinessTypeStore : ApplicationDbContextStore
    {
        public BusinessTypeStore(ApplicationDbContext context) : base(context)
        {
        }

        public BusinessType FindByName(string name)
        {
            return _context.BusinessTypes.FirstOrDefault(type => type.Name == name);
        }

        public async Task<BusinessType> FindByNameAsync(string name)
        {
            return await _context.BusinessTypes.FirstOrDefaultAsync(type => type.Name == name);
        } 
    }
}