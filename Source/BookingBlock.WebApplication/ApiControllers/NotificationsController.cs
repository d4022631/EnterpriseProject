using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Http;
using BookingBlock.EntityFramework;
using Microsoft.AspNet.Identity;

namespace BookingBlock.WebApplication.ApiControllers
{
    public static class StringExtensionMethods
    {
        public static string ReplaceFirst(this string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }
    }

    public class SimpleBooking
    {
        private string _phoneNumber;
        public DateTime DateTime { get; set; }

        public string Name { get; set; }

        public string For { get; set; }

        public string With { get; set; }

        public string PhoneNumber
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_phoneNumber))
                {
                    if (!_phoneNumber.StartsWith("+"))
                    {
                        return _phoneNumber.ReplaceFirst("0", "+44");
                    }    
                }

                return _phoneNumber;
            }
            set
            {
              
                _phoneNumber = value;
            }
        }

        public string EmailAddress { get; set; }
    }
    

    [System.Web.Http.RoutePrefix("api/Notifications")]
    public class NotificationsController : BaseApiController
    {

        private IEnumerable<SimpleBooking> GetBookings()
        {
            List<SimpleBooking> simpleBookings = new List<SimpleBooking>();


            // what time is it now.
            DateTime currentDateTime = DateTime.Now.AddHours(1);
            DateTime older = currentDateTime.AddHours(2);

            var bookings = db.Bookings.Where(
                booking => booking.Date >= currentDateTime && booking.Date <= older)
                .Select(s => new SimpleBooking()
                {
                    DateTime = s.Date,
                    For = s.Service.Name,
                    Name = s.Customer.FirstName,
                    With = s.Service.Business.Name,
                    PhoneNumber = s.Customer.PhoneNumber,
                    EmailAddress = s.Customer.Email
                })
                .ToList();

            simpleBookings.AddRange(bookings);

            return simpleBookings;
        }

        [HttpGet, Route("all-reminders")]
        public async Task<IHttpActionResult> RemindersList()
        {
            return Ok(GetBookings());
        }

        [HttpGet, Route("send-reminders")]
        public async Task<IHttpActionResult> Reminders()
        {
            try
            {
                EmailService emailService = new EmailService();

                SmsService smsService = new SmsService();

                foreach (SimpleBooking simpleBooking in GetBookings())
                {
                    string message =
                        $"Dear, {simpleBooking.Name}, you have an appointment with {simpleBooking.With}, for a {simpleBooking.For} at {simpleBooking.DateTime}";

                    if (!string.IsNullOrWhiteSpace(simpleBooking.EmailAddress))
                    {
                        try
                        {
                            await
    emailService.SendAsync(new IdentityMessage()
    {
        Body = message,
        Destination = simpleBooking.EmailAddress,
        Subject = "Booking Reminder"
    });
                        }
                        catch (Exception exception)
                        {

                           
                        }

                    }

                    if (!string.IsNullOrWhiteSpace(simpleBooking.PhoneNumber))
                    {
                        try
                        {
                            var client = new MScience.Sms.SmsClient
                            {
                                //
                                AccountId = ConfigurationManager.AppSettings["DragonFlyAccountId"],
                                Password = ConfigurationManager.AppSettings["DragonFlyPassword"]
                            };
                            var sendResult = client.Send(simpleBooking.PhoneNumber, "", message, 0, true);
                        }
                        catch (Exception exception)
                        {

                            return Content(HttpStatusCode.BadGateway, exception.Message);
                        }
                    }

                }
    
          

                return Ok(GetBookings());
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