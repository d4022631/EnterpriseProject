using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Routing;
using BookingBlock.EntityFramework;
using BookingBlock.WebApplication.Models;

namespace BookingBlock.WebApplication.ApiControllers
{
    public class BookingsStore
    {
        private readonly ApplicationDbContext _context;

        public BookingsStore(ApplicationDbContext context)
        {
            _context = context;
        }
    }

    public abstract class ManagerResult
    {
        //
        // Summary:
        //     True if the operation was successful
        public bool Succeeded { get; }

        //
        // Summary:
        //     List of errors
        public IEnumerable<string> Errors { get; }

    }

    public sealed class BookingsManagerResult : ManagerResult
    {
        
    }


    public class BookingsManager
    {
        private readonly BookingsStore _bookingsStore;

        public BookingsManager(BookingsStore bookingsStore)
        {
            _bookingsStore = bookingsStore;
        }

        public BookingsManagerResult CreateBookingAsync(string userId, Guid serviceId, DateTime dateTime, string notes)
        {
            return new BookingsManagerResult() {};
        }
    }

    //name, cost, time, duration




    [RoutePrefix("api/bookings")]
    public class BookingsController : BaseApiController
    {
        public async Task<IHttpActionResult> GetMyBookings()
        {
            return
                Ok(
                    db.Bookings.Where(booking => booking.CustomerId == UserId)
                        .Select(
                            s =>
                                new
                                {
                                    Date = s.Date,
                                    Duration = s.Duration,
                                    Name = s.Service.Name,
                                    TotalCost = s.TotalCost
                                })
                        .ToList());
        }

        [Route("bookings-old")]
        public async Task<IHttpActionResult> GetMyBookings2()
        {
            return Ok(db.Bookings.Where(booking => booking.CustomerId == UserId).ToList());
        }


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

            BookingsStore bookingsStore = new BookingsStore(db);
            BookingsManager bookingsManager = new BookingsManager(bookingsStore);

            bookingsManager.CreateBookingAsync(this.UserId, service.Id, createBookingRequest.DateTime, string.Empty);

            Booking booking = new Booking() {CustomerId = this.UserId, Date = createBookingRequest.DateTime};

            booking.TotalCost = service.Cost;
            booking.Duration = service.Duration;

            string logEntry = "Booking created.";

            booking.Log.Add(new BookingLog() { Entry = logEntry});

            service.Bookings.Add(booking);

            db.SaveChanges();

            return Ok();
        } 
    }
}