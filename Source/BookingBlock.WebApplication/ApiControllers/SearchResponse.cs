using System.Collections.Generic;

namespace BookingBlock.WebApplication.ApiControllers
{
    public class SearchResponse
    {
        public string BusinessType { get; set; }
        public string Postcode { get; set; }
        public double Within { get; set; }

        public IEnumerable<BusinessSearchResult> Results { get; set; }
    }
}