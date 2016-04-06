using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using BookingBlock.WebApplication.Models;

namespace BookingBlock.WebApplication.ApiControllers
{
    public abstract class BusinessTypeRequest
    {
        /// <summary>
        /// The name of the business type
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// An object to represent a business type we want to add to the database.
    /// </summary>
    public class AddBusinessTypeRequest : BusinessTypeRequest
    {
        
    }

    /// <summary>
    /// An object to represent a business type we want to remove from the database.
    /// </summary>
    public class RemoveBusinessTypeRequest : BusinessTypeRequest
    {

    }

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

        /// <summary>
        /// Add a business type to the database.
        /// </summary>
        /// <param name="businessType"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/BusinessTypes/Add")]
        public IHttpActionResult Add(AddBusinessTypeRequest businessType)
        {
            if (string.IsNullOrWhiteSpace(businessType.Name))
            {
                return BadRequest("No business type given");
            }

            var existing = context.BusinessTypes.FirstOrDefault(type => type.Name == businessType.Name);

            if (existing != null)
            {
                return Conflict();
            }

            context.BusinessTypes.Add(new BusinessType() {Name = businessType.Name});

            context.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Remove a business type from the database.
        /// </summary>
        /// <param name="businessType"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/BusinessTypes/Remove")]
        public IHttpActionResult Remove(RemoveBusinessTypeRequest businessType)
        {
            if (string.IsNullOrWhiteSpace(businessType.Name))
            {
                return BadRequest("No business type given");
            }

            var existing = context.BusinessTypes.FirstOrDefault(type => type.Name == businessType.Name);

            if (existing == null)
            {
                return BadRequest("Can't find the given business type to remove it");
            }

            if (context.Businesses.Any(business => business.BusinessTypeId == existing.Id))
            {
                return BadRequest("Cannot remove business type with business associated.");
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