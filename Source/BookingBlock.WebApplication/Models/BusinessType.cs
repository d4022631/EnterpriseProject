using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Linq;
using BookingBlock.WebApplication.Models.ValidationAttributes;

namespace BookingBlock.WebApplication.Models
{
    public class BusinessTypesStore
    {
        private readonly ApplicationDbContext _context;

        public BusinessTypesStore(ApplicationDbContext context)
        {
            _context = context;
        }

        public BusinessType FindByCode(string code)
        {
            return _context.BusinessTypes.FirstOrDefault(type => type.Code == code);
        }
    }

    public class Booking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public virtual ApplicationUser User { get; set; }

        [Required]
        public virtual Service Service { get; set; }

        public DateTime DateTime { get; set; }
    }

    public class Service
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public virtual Business Business { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; } 
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

        public virtual ICollection<ApplicationUser> Users { get; set; } 

        public virtual ICollection<Service> Services { get; set; } 

        [Required]
        public string Postcode { get; set; }

        [Required]
        public DbGeography Location { get; set; }
    }

    public class BusinessType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [StringLength(3), BusinessTypeCodeValidator]
        public string Code { get; set; }



        [Required]
        public string Name { get; set; }

        public virtual ICollection<Business> Businesses { get; set; } 
    }
}