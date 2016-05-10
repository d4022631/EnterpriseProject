using System;
using System.Configuration;
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

        [HttpGet, Route("send-reminders")]
        public async Task<IHttpActionResult> Reminders()
        {
            try
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

                        throw;
                    }
                }


                return Ok();
            }
            catch (Exception exception)
            {

                return Ok(exception.Message);
            }
        }

        [HttpGet, Route("sms-test")]
        public async Task<IHttpActionResult> Sms(string phoneNumber, string text)
        {
            var client = new MScience.Sms.SmsClient
            {
                //
                AccountId = ConfigurationManager.AppSettings["DragonFlyAccountId"],
                Password = ConfigurationManager.AppSettings["DragonFlyPassword"]
            };
            var sendResult = client.Send(phoneNumber, "", text, 0, true);


            if (sendResult.HasError)
            {
                BadRequest();
            }

            var deliveryReceipts = client.GetDeliveryReceipts();

            return Ok(deliveryReceipts);
        }


        [HttpGet, Route("email-test")]
        public async Task<IHttpActionResult> SendEmail(string address)
        {

            var myMessage = new SendGrid.SendGridMessage();
            myMessage.AddTo(address);
            myMessage.From = new MailAddress("Reminders@bookingblock.com", "BookingBlock Reminders");
            myMessage.Subject = "You have an upcoming appointment";
            myMessage.Text = "You have a booking @ " + DateTime.Now;

            var transportWeb =
                new SendGrid.Web(new NetworkCredential()
                {
                    UserName = ConfigurationManager.AppSettings["SendGridUsername"],
                    Password = ConfigurationManager.AppSettings["SendGridPassword"]
                });
            await transportWeb.DeliverAsync(myMessage);

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
            myMessage.Text = "You have a booking @ " + userBooking.Date;

            if (!string.IsNullOrWhiteSpace(user.PhoneNumber))
            {
                try
                {
                    await Sms(user.PhoneNumber, myMessage.Text);
                }
                catch (Exception)
                {
                    
                   // throw;
                }
            }

            var transportWeb =
                new SendGrid.Web(new NetworkCredential()
                {
                    UserName = ConfigurationManager.AppSettings["SendGridUsername"],
                    Password = ConfigurationManager.AppSettings["SendGridPassword"]
                });
            await transportWeb.DeliverAsync(myMessage);

            return Ok();
        } 
    }
}