using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using BookingBlock.WebApplication.Models;
using MarkEmbling.PostcodesIO;

namespace BookingBlock.WebApplication.ApiControllers
{
    [RoutePrefix("api/businesses")]
    public class BusinessController : ApiController
    {
        [HttpPost, Route("register")]
        public async Task<IHttpActionResult> Register(AddBusinessRequest addBusinessRequest)
        {
            try
            {
                ApplicationDbContext context = new ApplicationDbContext();

                var wm = context.Users.FirstOrDefault(user => user.Email == "webmaster@bookingblock.azurewebsites.net");

                
                PostcodesIOClient client = new PostcodesIOClient();

                var t = client.Random();
                
                var bt = context.BusinessTypes.OrderBy(r => Guid.NewGuid()).Take(1).FirstOrDefault();

                Business business = new Business();

                business.Postcode = t.Postcode;
                business.Location = GeoUtils.CreatePoint(t.Latitude, t.Longitude);

                business.Users.Add(new BusinessUser() {User = wm});
                business.BusinessType = bt;
                business.Name = "Webmaster's " + bt.Name + " [" + t.Postcode +  "]";

                context.Businesses.Add(business);

                context.SaveChanges();
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }


            return Ok();
        }
    }
}