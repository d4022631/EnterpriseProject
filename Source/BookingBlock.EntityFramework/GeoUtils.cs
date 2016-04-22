using System;
using System.Data.Entity.Spatial;

namespace BookingBlock.EntityFramework
{
    public static class GeoUtils
    {
        /// 

        /// Convert meters to miles
        /// 

        /// 
        /// 
        public static double MetersToMiles(double? meters)
        {
            if (meters == null)
                return 0F;

            return meters.Value * 0.000621371192;
        }

        /// 

        /// Convert miles to meters
        /// 

        /// 
        /// 
        public static double MilesToMeters(double? miles)
        {
            if (miles == null)
                return 0;

            return miles.Value * 1609.344;
        }

        /// <summary>
        /// Create a GeoLocation point based on latitude and longitude
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        public static DbGeography CreatePoint(double latitude, double longitude)
        {
            var text = string.Format("POINT({0} {1})", longitude, latitude);
            // 4326 is most common coordinate system used by GPS/Maps
            return DbGeography.PointFromText(text, 4326);
        }

        /// <summary>
        /// Create a GeoLocation point based on latitude and longitude
        /// </summary>
        /// <param name="latitudeLongitude">
        /// String should be two values either single comma or space delimited
        /// 45.710030,-121.516153
        /// 45.710030 -121.516153
        /// </param>
        /// <returns></returns>
        public static DbGeography CreatePoint(string latitudeLongitude)
        {
            var tokens = latitudeLongitude.Split(',', ' ');
            if (tokens.Length != 2)
                throw new ArgumentException("Invalid location");
            var text = string.Format("POINT({0} {1})", tokens[1], tokens[0]);
            return DbGeography.PointFromText(text, 4326);
        }
    }
}