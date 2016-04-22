using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using BookingBlock.EntityFramework;
using BookingBlock.WebApplication.Models;

namespace BookingBlock.WebApplication.Controllers
{
    public class DatabaseController : Controller
    {
        // GET: Database
        public ActionResult Edmx()
        {
            var r = Path.GetTempFileName();

            using (var ctx =  ApplicationDbContext.Create())
            {
                using (var writer = new XmlTextWriter(r, Encoding.Default))
                {
                    EdmxWriter.WriteEdmx(ctx, writer);
                }
            }

            return File(r, "text/xml", Path.GetRandomFileName() + ".edmx");
        }
    }
}