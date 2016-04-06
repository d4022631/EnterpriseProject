using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace BookingBlock.WebApplication.Models
{
    public class OpeningTimes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public virtual Business Business { get; set; }

        [Required, ForeignKey(nameof(Business))]
        public Guid BusinessId { get; set; }

        [Required]
        public DayOfWeek DayOfWeek { get; set; }

        [Required]
        public TimeSpan OpeningTime { get; set; }

        [Required]
        public TimeSpan ClosingTime { get; set; }
    }


    public class Business
    {
        public Business()
        {
            this.Services = new List<Service>();
            this.Users = new List<ApplicationUser>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public virtual BusinessType BusinessType { get; set; }

        [Required, ForeignKey(nameof(BusinessType))]
        public Guid BusinessTypeId { get; set; }


        [Required]
        public string Postcode { get; set; }

        [Required]
        public DbGeography Location { get; set; }



        public virtual ICollection<ApplicationUser> Users { get; set; }

        public virtual ICollection<Service> Services { get; set; }

        public virtual ICollection<OpeningTimes> OpeningTimes { get; set; }
    }
}