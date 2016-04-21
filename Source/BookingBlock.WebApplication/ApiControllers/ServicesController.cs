using System.Web.Http;

namespace BookingBlock.WebApplication.ApiControllers
{
    [RoutePrefix("api/services")]
    public class ServicesController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Add(AddServiceRequest addServiceRequest)
        {
            return Ok();
        }
    }
}