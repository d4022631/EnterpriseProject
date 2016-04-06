using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using BookingBlock.WebApplication.Models;

namespace BookingBlock.WebApplication.ApiControllers
{
    public class BusinessTypesController : ApiController
    {
        private ApplicationDbContext context = ApplicationDbContext.Create();

        [HttpGet]
        public async Task<IHttpActionResult> GetAll()
        {
            ApplicationDbContext context = ApplicationDbContext.Create();

            var allBusinessTypes = await context.BusinessTypes.Select(type => type.Name).ToListAsync();

            return Ok(allBusinessTypes);
        }

        [HttpPost]
        [Route("api/BusinessTypes/Add")]
        public IHttpActionResult Add(string businessType)
        {
            if (string.IsNullOrWhiteSpace(businessType))
            {
                return BadRequest("No business type given");
            }

            var existing = context.BusinessTypes.FirstOrDefault(type => type.Name == businessType);

            if (existing != null)
            {
                return Conflict();
            }

            context.BusinessTypes.Add(new BusinessType() {Name = businessType});

            context.SaveChanges();

            return Ok();
        }

        [HttpPost]
        [Route("api/BusinessTypes/Remove")]
        public IHttpActionResult Remove(string businessType)
        {
            if (string.IsNullOrWhiteSpace(businessType))
            {
                return BadRequest("No business type given");
            }

            var existing = context.BusinessTypes.FirstOrDefault(type => type.Name == businessType);

            if (existing == null)
            {
                return BadRequest("Can't find the given business type to remove it");
            }

            context.BusinessTypes.Remove(existing);

            context.SaveChanges();

            return Ok();
        }

        [HttpGet]
        [Route("api/BusinessTypes/AutoComplete/{businessType}")]
        public IHttpActionResult AutoComplete(string businessType)
        {


            return Ok(Match(businessType));
        }

        private IEnumerable<string> Match(string text, double min = 0.3)
        {
            return context.BusinessTypes.Where(b => b.Name.Contains(text)).Select(b => b.Name);
        }
    }
}