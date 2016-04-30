using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Http;
using BookingBlock.EntityFramework;

namespace BookingBlock.WebApplication.ApiControllers
{
    [System.Web.Http.RoutePrefix("api/Notifications")]
    public class NotificationsController : BaseApiController
    {

        [HttpGet, Route("Reminders")]
        public async Task<IHttpActionResult> Reminders()
        {
            // what time is it now.
            DateTime currentDateTime = DateTime.Now;

            var bookings = db.Bookings.Where(
                booking =>
                    !booking.Cancelled && booking.Date >= currentDateTime && booking.Date <= currentDateTime.AddHours(1));


            foreach (Booking booking in bookings)
            {
                try
                {
                    await Send(booking.Id);
                }
                catch (Exception)
                {
                    
                    //throw;
                }
            }


            return Ok();
        }

        [HttpGet, Route("Booking-Reminder")]
        public async Task<IHttpActionResult> Send(Guid id)
        {
            var userBooking =  await db.Bookings.FirstOrDefaultAsync(booking => booking.Id == id);


            if (userBooking == null)
            {
                return NotFound();
            }

            var user =
                await db.Users.FirstOrDefaultAsync(applicationUser => applicationUser.Id == userBooking.CustomerId);

            if (user == null)
            {
                return NotFound();
            }

            
            var myMessage = new SendGrid.SendGridMessage();
            myMessage.AddTo(user.Email);
            myMessage.From = new MailAddress("Reminders@bookingblock.com", "BookingBlock Reminders");
            myMessage.Subject = "You have an upcoming appointment";
            myMessage.Text = "You have a booking @ " + userBooking.Date ;

            var transportWeb =
                new SendGrid.Web(new NetworkCredential()
                {
                    UserName = "azure_c6a96ba7b269278355a559b27a41d6d4@azure.com",
                    Password = "Enterprise2016!"
                });
            await transportWeb.DeliverAsync(myMessage);

            return Ok();
        } 
    }
}