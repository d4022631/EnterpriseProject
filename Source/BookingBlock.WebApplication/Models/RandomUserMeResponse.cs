using System.Collections.Generic;

namespace BookingBlock.WebApplication.Models
{
    public class RandomUserMeResponse
    {
        public List<RandomUserMeResult> results { get; set; }
        public RandomUserMeInfo info { get; set; }
    }
}