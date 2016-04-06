using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
}