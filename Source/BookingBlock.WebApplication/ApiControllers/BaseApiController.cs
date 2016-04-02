using System.Net;
using System.Web.Http;
using System.Web.Http.Results;

namespace BookingBlock.WebApplication.ApiControllers
{
    public abstract class BaseApiController : ApiController
    {
        internal IHttpActionResult Response<T>(HttpStatusCode statusCode, T content) where T : ApiResponse
        {
            return Content(statusCode, content);
        }
    }
}