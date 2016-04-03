using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using BookingBlock.WebApplication.Models;

namespace BookingBlock.WebApplication.ApiControllers
{
    public class BusinessTypesController : ApiController
    {
        [HttpGet]
        [Route("api/BusinessTypes/AutoComplete/{businessType}")]
        public IHttpActionResult Get(string businessType)
        {


            return Ok(Match(businessType));
        }

        private IEnumerable<string> Match(string text, double min = 0.3)
        {
            var c = ApplicationDbContext.Create();

            return c.BusinessTypes.Where(b => b.Name.Contains(text)).Select(b => b.Name);
        }
    }
}