using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using BookingBlock.EntityFramework;
using BookingBlock.WebApplication.Models;

namespace BookingBlock.WebApplication.ApiControllers
{
    public class BusinessTypesController : BaseApiController
    {
        private ApplicationDbContext context = ApplicationDbContext.Create();

        [HttpGet]
        public async Task<IHttpActionResult> GetAll()
        {
            ApplicationDbContext context = ApplicationDbContext.Create();

            var allBusinessTypes = await context.BusinessTypes.Select(type => type.Name).ToListAsync();

            return Ok(allBusinessTypes);
        }

        [HttpGet]
        [Route("api/BusinessTypes/FixDuplicated")]
        public async Task<IHttpActionResult> FixDuplicate(string name)
        {
            var duplicates = db.BusinessTypes.Where(type => type.Name == name).ToList();

            var idsss = duplicates.Select(type => type.Id).ToList();

            var du = (from order in db.Businesses
                where (idsss.Contains(order.BusinessTypeId))
                select order);

            var id = idsss.FirstOrDefault();

            foreach (Business business in du)
            {
                business.BusinessTypeId = id;
            }

            foreach (BusinessType businessType in duplicates)
            {
                if (businessType.Id != id)
                {
                    db.BusinessTypes.Remove(businessType);
                }
            }



            var a = db.SaveChanges();
            return Ok(name);
        }

        [HttpGet]
        [Route("api/BusinessTypes/FixDuplicatedAuto")]
        public async Task<IHttpActionResult> FixDuplicateAuto()
        {
            var groups = db.BusinessTypes.GroupBy(type => type.Name);

            Dictionary<string, int> dups = new Dictionary<string, int>();

            foreach (IGrouping<string, BusinessType> businessTypes in groups)
            {
                if (businessTypes.Count() > 1)
                {
                    dups.Add(businessTypes.Key, businessTypes.Count());
                }
            }

            var d = dups.FirstOrDefault().Key;

            return await FixDuplicate(d);

        }

        [HttpGet]
        [Route("api/BusinessTypes/Duplicated")]
        public async Task<IHttpActionResult> Duplicates()
        {
            var groups = db.BusinessTypes.GroupBy(type => type.Name);

            Dictionary<string, int> dups = new Dictionary<string, int>();

            foreach (IGrouping<string, BusinessType> businessTypes in groups)
            {
                if (businessTypes.Count() > 1)
                {
                    dups.Add(businessTypes.Key, businessTypes.Count());
                }
            }

            return Ok(dups);
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