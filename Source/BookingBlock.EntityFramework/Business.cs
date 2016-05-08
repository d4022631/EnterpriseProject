using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Threading.Tasks;

namespace BookingBlock.EntityFramework
{
    public class BusinessStore
    {
        readonly ApplicationDbContext _context;

        public BusinessStore(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Business> FindAsync(Guid businessId)
        {
            return await _context.Businesses.FindAsync(businessId);
        }

        public async Task<bool> DeleteAsync(Guid businessId)
        {
            var business = await FindAsync(businessId);

            if (business != null)
            {
                if (!business.Deleted)
                {
                    business.Deleted = true;
                    business.Modified = DateTime.Now;
                    
                    return _context.SaveChanges() > 0;
                }

                return true;
         
            }

            return false;
        } 
    }

    public class Business : BookingBlockEntity
    {
        public Business()
        {
            this.Services = new List<Service>();
            this.Users = new List<BusinessUser>();
            this.OpeningTimes = new List<BusinessOpeningTime>();
        }

        [Required]
        public string Name { get; set; }

        [Required, DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [Required]
        public string Postcode { get; set; }

        public string PhoneNumber { get; set; }

        public string FaxNumber { get; set; }

        [Url]
        public string Website { get; set; }

        public string Facebook { get; set; }

        public string LinkedIn { get; set; }

        public string GooglePlus { get; set; }

        public DateTime RegistrationDate { get; set; } = DateTime.Now;



        [EmailAddress]
        public string EmailAddress { get; set; }

        [DataType(DataType.Url)]
        public string LogoUrl { get; set; }

        [Required]
        public DbGeography Location { get; set; } = GeoUtils.CreatePoint(0, 0);

        [NotMapped]
        public double Longitude => Location?.Longitude ?? 0;

        [NotMapped]
        public double Latitude => Location?.Latitude ?? 0;

        public virtual BusinessType BusinessType { get; set; }

        [Required, ForeignKey(nameof(BusinessType))]
        public Guid BusinessTypeId { get; set; }

        public virtual ICollection<BusinessUser> Users { get; set; }

        public virtual ICollection<Service> Services { get; set; }

        public virtual ICollection<BusinessOpeningTime> OpeningTimes { get; set; }


    }
}