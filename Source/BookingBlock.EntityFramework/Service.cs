using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingBlock.EntityFramework
{
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
}