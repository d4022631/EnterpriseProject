using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Threading.Tasks;
using BookingBlock.EntityFramework;
using MarkEmbling.PostcodesIO;

namespace BookingBlock.WebApplication.ApiControllers
{
    public class PostcodesService
    {
        public static DbGeography Lookup(string postcode)
        {

            PostcodesIOClient client = new PostcodesIOClient();

            var postcodeLookup = client.Lookup(postcode);

            // use the GeoUtils class to create a DbGeography object to represent the point.
            return GeoUtils.CreatePoint(postcodeLookup.Latitude, postcodeLookup.Longitude);
        }

        public async Task<IEnumerable<string>> AutoCompleteAsync(string postcode)
        {
            PostcodesIOClient client = new PostcodesIOClient();

            var result = await client.AutocompleteAsync(postcode);

            return result;
        }

        public async Task<bool> ValidateAsync(string postcode)
        {
            PostcodesIOClient client = new PostcodesIOClient();

            var result = await client.ValidateAsync(postcode);

            return result;
        }
    }
}