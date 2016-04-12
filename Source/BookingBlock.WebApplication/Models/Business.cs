using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace BookingBlock.WebApplication.Models
{

    /// <summary>
    /// Represents media that can be added to a business page, videos, pdfs, images, etc...
    /// </summary>
    public class Media
    {
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Business Business {get; set;}

        [Required, ForeignKey(nameof(Business))]
        public Guid BusinessId {get; set;}

        public string Url {get; set;}
    
        public string Description {get; set;}

        public string Title {get; set;}
    }

    public class Business
    {
        public Business()
        {
            this.Services = new List<Service>();
            this.Users = new List<BusinessUser>();
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

        [DataType(DataType.Url)]
        public string LogoUrl {get; set;}

        [Required]
        public DbGeography Location { get; set; } = GeoUtils.CreatePoint(0, 0);

        public virtual BusinessType BusinessType { get; set; }

        [Required, ForeignKey(nameof(BusinessType))]
        public Guid BusinessTypeId { get; set; }

        public virtual ICollection<BusinessUser> Users { get; set; }

        public virtual ICollection<Service> Services { get; set; }

        public virtual ICollection<BusinessOpeningTime> OpeningTimes { get; set; }
    }

    public class BusinessUser
    {
      
        public virtual Business Business { get; set; }

        
        public virtual ApplicationUser User { get; set; }

        [Required]
        [Key, Column(Order = 0), ForeignKey(nameof(Business))]
        public Guid BusinessId { get; set; }

        [Required]
        [Key, Column(Order = 1), ForeignKey(nameof(User))]
        public string UserId { get; set; }

        [Required]
        public BusinessUserLevel UserLevel { get; set; }
    }

    public enum BusinessUserLevel
    {
        Owner,
        Staff
    }


}