using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace BookingBlock.WebApplication.Models
{
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

        [Required]
        public string Address { get; set; }

        [Required]
        public string Postcode { get; set; }

        public string PhoneNumber { get; set; }

        public string FaxNumber { get; set; }

        public string Website { get; set; }

        public string Facebook { get; set; }

        public string LinkedIn { get; set; }

        public string GooglePlus { get; set; }


        [Required]
        public DbGeography Location { get; set; }

        [Required]
        public virtual BusinessType BusinessType { get; set; }

        [Required, ForeignKey(nameof(BusinessType))]
        public Guid BusinessTypeId { get; set; }

        public virtual ICollection<BusinessUser> Users { get; set; }

        public virtual ICollection<Service> Services { get; set; }

        public virtual ICollection<OpeningTimes> OpeningTimes { get; set; }
    }

    public class BusinessUser
    {
      
        public virtual Business Business { get; set; }

        
        public virtual ApplicationUser User { get; set; }

        [Key, Column(Order = 0), ForeignKey(nameof(Business))]
        public Guid BusinessId { get; set; }

        [Key, Column(Order = 1), ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public BusinessUserLevel UserLevel { get; set; }
    }

    public enum BusinessUserLevel
    {
        Owner,
        Staff
    }
}