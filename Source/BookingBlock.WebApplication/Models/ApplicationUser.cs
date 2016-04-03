using System;
using System.Data.Entity.Spatial;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BookingBlock.WebApplication.Models
{
    public static class GeoUtils
    {
        /// <summary>
        /// Create a GeoLocation point based on latitude and longitude
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        public static DbGeography CreatePoint(double latitude, double longitude)
        {
            var text = string.Format("POINT({0} {1})", longitude, latitude);
            // 4326 is most common coordinate system used by GPS/Maps
            return DbGeography.PointFromText(text, 4326);
        }

        /// <summary>
        /// Create a GeoLocation point based on latitude and longitude
        /// </summary>
        /// <param name="latitudeLongitude">
        /// String should be two values either single comma or space delimited
        /// 45.710030,-121.516153
        /// 45.710030 -121.516153
        /// </param>
        /// <returns></returns>
        public static DbGeography CreatePoint(string latitudeLongitude)
        {
            var tokens = latitudeLongitude.Split(',', ' ');
            if (tokens.Length != 2)
                throw new ArgumentException("Invalid location");
            var text = string.Format("POINT({0} {1})", tokens[1], tokens[0]);
            return DbGeography.PointFromText(text, 4326);
        }
    }


    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Title { get; set; }

        public DateTime DateOfBirth { get; set; } = new DateTime(1900,1,1,0,0,0);

        public string Postcode { get; set; }

        public string Address { get; set; }

        public DbGeography Location { get; set; }


        public ApplicationUser()
        {
            this.Location = GeoUtils.CreatePoint(0, 0);
        }
    }
}