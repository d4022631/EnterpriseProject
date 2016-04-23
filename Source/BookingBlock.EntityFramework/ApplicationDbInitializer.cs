using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingBlock.EntityFramework.Migrations;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BookingBlock.EntityFramework
{
    public class ApplicationDbInitializer : MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>
    {
       
    }
}
