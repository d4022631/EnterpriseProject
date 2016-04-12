using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using BookingBlock.WebApplication.Models.ValidationAttributes;

namespace BookingBlock.WebApplication.Models
{




    public class Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        
        public virtual Business Business  { get; set; }

        [Required, ForeignKey(nameof(Business ))]
        public Guid BusinessId { get; set; }

        [Required]
        public int Rating { get; set; }

        [Required]
        public string Comments { get; set; }

    }

    public class Booking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }


        public virtual Service Service { get; set; }

        [Required, ForeignKey(nameof(Service))]
        public Guid ServiceId { get; set; }

        public virtual ApplicationUser Customer { get; set; }

        [Required, ForeignKey(nameof(Customer))]
        public string CustomerId { get; set; }


        [Required, DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        [Required, DataType(DataType.Duration)]
        public TimeSpan Duration { get; set; }

        [Required, DataType(DataType.Currency)]
        public decimal TotalCost { get; set; }

        public string Notes { get; set; }


        public ICollection<Review> Reviews {get; set;}

        public bool Confirmed { get; set; }
        public bool Cancelled { get; set; }

        public bool Amended { get; set; }

        public bool Attended { get; set; }
    }


    public class Service
    {
        public Service()
        {
            this.Bookings = new List<Booking>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

       
        public virtual Business Business { get; set; }

        [Required, ForeignKey(nameof(Business))]
        public Guid BusinessId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public decimal Cost { get; set; } = 0.0M;

        [Required]
        public TimeSpan Duration { get; set; }



        public virtual ICollection<Booking> Bookings { get; set; } 
    }


    public class BusinessType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<Business> Businesses { get; set; } 
    }
}