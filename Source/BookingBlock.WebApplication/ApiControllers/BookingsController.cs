using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using BookingBlock.WebApplication.Models;

namespace BookingBlock.WebApplication.ApiControllers
{
    [RoutePrefix("api/bookings")]
    public class BookingsController : BaseApiController
    {
        [Route("create")]
        public async Task<IHttpActionResult> Create(CreateBookingRequest createBookingRequest)
        {
            if (createBookingRequest == null)
            {
                return Content(HttpStatusCode.BadRequest, "No booking information was given with the request.");
            }

            if (!ModelState.IsValid)
            {
                return InvalidModel();
            }

            if (!IsUserAuthenticated)
            {
                return Content(HttpStatusCode.Unauthorized, "You must be logged in to create a booking.");
            }

            var service = db.Services.FirstOrDefault(service1 => service1.Id == createBookingRequest.ServiceId);

            if (service == null)
            {
                return Content(HttpStatusCode.NotFound, $"The service {createBookingRequest.ServiceId} could not be found.");
            }

            var business = db.Businesses.FirstOrDefault(business1 => business1.Id == service.BusinessId);

            if (business == null)
            {
                return Content(HttpStatusCode.NotFound,
                    $"Could not find the business {service.BusinessId} that belongs to the service {createBookingRequest.ServiceId}");
            }

            var dayOfWeek = createBookingRequest.DateTime.DayOfWeek;

            var openingTime = db.BusinessOpeningTimes.FirstOrDefault(time => time.BusinessId == business.Id && time.DayOfWeek == dayOfWeek);

            if (openingTime == null)
            {
                return Content(HttpStatusCode.BadRequest,
                    $"The business {business.Id} is not open on this day of the week {dayOfWeek}.");
            }

            var bookingTime = createBookingRequest.DateTime.TimeOfDay;

            if (bookingTime <= openingTime.OpeningTime || bookingTime >= openingTime.ClosingTime)
            {
                return Content(HttpStatusCode.BadRequest,
                    $"The business {business.Id} is not open at this time {bookingTime}.");
            }

            service.Bookings.Add(new Booking() {CustomerId = this.UserId, Date = createBookingRequest.DateTime});

            db.SaveChanges();

            return Ok();
        } 
    }
}