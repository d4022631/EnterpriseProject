using System;
using System.ComponentModel.DataAnnotations;

namespace BookingBlock.WebApplication.ApiControllers
{
    public class AddServiceRequest
    {
        [Required]
        public Guid BusinessId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public decimal Cost { get; set; }

        [Required]
        public TimeSpan Duration { get; set; }
    }

}