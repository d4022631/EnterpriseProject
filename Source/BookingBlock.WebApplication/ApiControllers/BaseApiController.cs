using System.Net;
using System.Web.Http;
using System.Web.Http.Results;
using BookingBlock.WebApplication.Models;

namespace BookingBlock.WebApplication.ApiControllers
{
    public abstract class BaseApiController : ApiController
    {
        protected ApplicationDbContext db = new ApplicationDbContext();

        internal IHttpActionResult Response<T>(HttpStatusCode statusCode, T content) where T : ApiResponse
        {
            return Content(statusCode, content);
        }
    }
}