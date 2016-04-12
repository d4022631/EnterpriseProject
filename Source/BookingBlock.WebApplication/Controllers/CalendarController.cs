using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookingBlock.WebApplication.Controllers
{
    public class CalendarController : Controller
    {

        public class CalendarEvent
        {
     
           public  string title {get; set;}
           public string description { get; set; }

           public string start { get; set; }

           public string end { get; set; }

           public string allday { get; set; }
           public string url { get; set; }

           public string color { get; set; }

           public string borderColor { get; set; }

           public string textColor { get; set; } 


        }
      //  http://www.mikesmithdev.com/blog/fullcalendar-json-feed-httphandler-csharp/
        // GET: Calendar
        public ActionResult Index()
        {
            List<CalendarEvent> events = new List<CalendarEvent>();

            events.Add(new CalendarEvent() { title = "TEST", start = DateTime.Now.ToString("yyyy-MM-dd HH:mm"), end=DateTime.Now.AddHours(2).ToString("yyyy-MM-dd HH:mm"});

            return View();
        }
    }
}