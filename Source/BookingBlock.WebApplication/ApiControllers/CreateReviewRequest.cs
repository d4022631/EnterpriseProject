using System;

namespace BookingBlock.WebApplication.ApiControllers
{
    public class CreateReviewRequest
    {
        public Guid BusinessId { get; set; }
        public int Rating { get; set; }
        public string Comments { get; set; }
    }
}