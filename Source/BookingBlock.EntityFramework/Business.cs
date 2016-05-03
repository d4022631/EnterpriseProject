using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace BookingBlock.EntityFramework
{
    public class Business
    {
        public Business()
        {
            this.Services = new List<Service>();
            this.Users = new List<BusinessUser>();
            this.OpeningTimes = new List<BusinessOpeningTime>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

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

        /// <summary>
        /// Gets or sets a flag to indicate if the entity a a dummy entity created for testing.
        /// </summary>
        public bool IsDummy { get; set; }
    }
}