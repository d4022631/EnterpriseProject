using System.Threading.Tasks;
using System.Web.Http;
using BookingBlock.EntityFramework;

namespace BookingBlock.WebApplication.ApiControllers
{
    [RoutePrefix("api/reviews")]
    public class ReviewsController : BaseApiController
    {
        [HttpPost, Route("create")]
        public async Task<IHttpActionResult> Create(CreateReviewRequest createReviewRequest)
        {
            db.Reviews.Add(new Review()
            {
                BusinessId = createReviewRequest.BusinessId,
                Comments = createReviewRequest.Comments,
                Rating = createReviewRequest.Rating
            });

            return Ok();
        } 
    }
}