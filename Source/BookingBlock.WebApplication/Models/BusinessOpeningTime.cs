using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingBlock.WebApplication.Models
{
    public class BusinessOpeningTime
    {
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