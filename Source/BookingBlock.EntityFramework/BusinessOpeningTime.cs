using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingBlock.EntityFramework
{
    public class BusinessOpeningTime
    {

        public DateTime Created { get; set; } = DateTime.Now;

        public DateTime Modified { get; set; } = DateTime.Now;

        public bool Deleted { get; set; } = false;

        public virtual Business Business { get; set; }

        [Required, ForeignKey(nameof(Business)), Key, Column(Order = 0)]
        public Guid BusinessId { get; set; }

        [Required, Key, Column(Order = 1)]
        public DayOfWeek DayOfWeek { get; set; }

        [Required]
        public TimeSpan OpeningTime { get; set; }

        [Required]
        public TimeSpan ClosingTime { get; set; }
    }
}