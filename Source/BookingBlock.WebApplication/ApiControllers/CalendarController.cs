
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BookingBlock.WebApplication.ApiControllers
{
    public class CalendarController : ApiController
    {

        public class CalendarEvent
        {

            public string title { get; set; }
            public string description { get; set; }

            public string start { get; set; }

            public string end { get; set; }

            public bool allDay { get; set; }
            public string url { get; set; }

            public string color { get; set; }

            public string borderColor { get; set; }

            public string textColor { get; set; }

        }

        public IHttpActionResult Get()
        {
            List<CalendarEvent> events = new List<CalendarEvent>();

            events.Add(new CalendarEvent() {
                title = "TEST",
                start = DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                end=DateTime.Now.AddHours(2).ToString("yyyy-MM-dd HH:mm")});
             
            return Ok(events);
        }
    }
}
