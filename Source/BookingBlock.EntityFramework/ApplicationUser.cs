using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BookingBlock.EntityFramework
{
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

        public DateTime DateOfBirth { get; set; } = new DateTime(1900, 1, 1, 0, 0, 0);

        public string Postcode { get; set; }

        public string Address { get; set; }

        public DbGeography Location { get; set; }

        public virtual ICollection<BusinessUser> Businesses { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }

        public Gender Gender { get; set; }

        /// <summary>
        /// Gets or sets the date the account was registered.
        /// </summary>
        public DateTime RegistrationDate { get; set; } = DateTime.Now;


        public DateTime Modified { get; set; } = DateTime.Now;

        public bool Deleted { get; set; } = false;

        public ApplicationUser()
        {
            this.Location = GeoUtils.CreatePoint(0, 0);
            this.Businesses = new List<BusinessUser>();
            this.Bookings = new List<Booking>();
        }
    }
}