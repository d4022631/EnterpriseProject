using System.Collections.Generic;

namespace BookingBlock.WebApplication.ApiControllers
{
    public class AgeDistribution
    {
        public Dictionary<int, int> Ages { get; set; } = new Dictionary<int, int>();
    }
}