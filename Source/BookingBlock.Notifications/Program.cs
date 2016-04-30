using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BookingBlock.EntityFramework;

namespace BookingBlock.Notifications
{
    class Program
    {
        static void Main(string[] args)
        {
            ApplicationDbContext applicationDbContext = new ApplicationDbContext();


            var timeOfDay = DateTime.Now.TimeOfDay;

            var hours = timeOfDay.Hours;
            var minutes = timeOfDay.Minutes;
            var seconds = 0;

            var dayOfWeek = DateTime.Now.Date;

            var start = dayOfWeek.Add(new TimeSpan(hours, minutes, seconds));

            var end = start.AddMinutes(15);

            var bookings =
                applicationDbContext.Bookings.Where(
                    booking => booking.Date >= start && booking.Date <= end && !booking.Cancelled);

            foreach (Booking booking in bookings)
            {

                try
                {
                    HttpClient client = new HttpClient();

                    client.GetAsync("https://localhost:44383/api/Notifications/" + booking.Id).GetAwaiter().GetResult();
                }
                catch (Exception)
                {
                    
                    throw;
                }

                Console.WriteLine("Notification sent.");

            }
        }
    }
}
