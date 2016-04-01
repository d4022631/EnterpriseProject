using System;
using System.Collections;
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

        public ActionResult AppSettings()
        {
            // get all the keys in the app settings section of the web.config.
            var allKeys = System.Configuration.ConfigurationManager.AppSettings.AllKeys;

            IDictionary appSettingsDictionary = new Dictionary<object, object>();

            foreach (string key in allKeys)
            {
                string value = System.Configuration.ConfigurationManager.AppSettings[key];

                appSettingsDictionary.Add(key, value);
            }

            return View(appSettingsDictionary);
        }
    }
}