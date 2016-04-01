using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookingBlock.WebApplication.Controllers
{
    public class EnvironmentController : Controller
    {
        // GET: Environment
        public ActionResult EnvironmentVariables()
        {
            var environmentVariables = Environment.GetEnvironmentVariables();

            return View(environmentVariables);
        }
    }
}