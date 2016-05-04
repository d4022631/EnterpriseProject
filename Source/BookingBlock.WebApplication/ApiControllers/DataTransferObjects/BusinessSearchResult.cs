using System;

namespace BookingBlock.WebApplication.ApiControllers
{
    public class BusinessSearchResult
    {
        public string Name { get; set; }

        public string Postcode { get; set; }

        public string Address { get; set; }

        public double Distance { get; set; }
        public Guid BusinessId { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string Website { get; set; }
    }
}