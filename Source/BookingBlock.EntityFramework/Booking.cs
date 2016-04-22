using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingBlock.EntityFramework
{
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


        public ICollection<Review> Reviews { get; set; }

        public bool Confirmed { get; set; }
        public bool Cancelled { get; set; }

        public bool Amended { get; set; }

        public bool Attended { get; set; }
    }
}