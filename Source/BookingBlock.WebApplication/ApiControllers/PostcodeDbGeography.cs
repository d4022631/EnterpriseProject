using System.Data.Entity.Spatial;
using BookingBlock.EntityFramework;
using BookingBlock.WebApplication.Models;
using MarkEmbling.PostcodesIO;

namespace BookingBlock.WebApplication.ApiControllers
{
    public static class PostcodeDbGeography
    {
        public static DbGeography Lookup(string postcode)
        {

            PostcodesIOClient client = new PostcodesIOClient();

            var postcodeLookup = client.Lookup(postcode);

            // use the GeoUtils class to create a DbGeography object to represent the point.
            return GeoUtils.CreatePoint(postcodeLookup.Latitude, postcodeLookup.Longitude);
        }
    }
}