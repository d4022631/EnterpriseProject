using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web.Http;
using BookingBlock.EntityFramework;
using BookingBlock.WebApplication.Models;
using MarkEmbling.PostcodesIO;
using Microsoft.AspNet.Identity;

namespace BookingBlock.WebApplication.ApiControllers
{
    public class SearchController : BaseApiController
    {

        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        //:::                                                                         :::
        //:::  This routine calculates the distance between two points (given the     :::
        //:::  latitude/longitude of those points). It is being used to calculate     :::
        //:::  the distance between two locations using GeoDataSource(TM) products    :::
        //:::                                                                         :::
        //:::  Definitions:                                                           :::
        //:::    South latitudes are negative, east longitudes are positive           :::
        //:::                                                                         :::
        //:::  Passed to function:                                                    :::
        //:::    lat1, lon1 = Latitude and Longitude of point 1 (in decimal degrees)  :::
        //:::    lat2, lon2 = Latitude and Longitude of point 2 (in decimal degrees)  :::
        //:::    unit = the unit you desire for results                               :::
        //:::           where: 'M' is statute miles (default)                         :::
        //:::                  'K' is kilometers                                      :::
        //:::                  'N' is nautical miles                                  :::
        //:::                                                                         :::
        //:::  Worldwide cities and other features databases with latitude longitude  :::
        //:::  are available at http://www.geodatasource.com                          :::
        //:::                                                                         :::
        //:::  For enquiries, please contact sales@geodatasource.com                  :::
        //:::                                                                         :::
        //:::  Official Web site: http://www.geodatasource.com                        :::
        //:::                                                                         :::
        //:::           GeoDataSource.com (C) All Rights Reserved 2015                :::
        //:::                                                                         :::
        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

        private double distance(double lat1, double lon1, double lat2, double lon2, char unit)
        {
            double theta = lon1 - lon2;
            double dist = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(theta));
            dist = Math.Acos(dist);
            dist = rad2deg(dist);
            dist = dist * 60 * 1.1515;
            if (unit == 'K')
            {
                dist = dist * 1.609344;
            }
            else if (unit == 'N')
            {
                dist = dist * 0.8684;
            }
            return (dist);
        }

        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        //::  This function converts decimal degrees to radians             :::
        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        private double deg2rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }

        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        //::  This function converts radians to decimal degrees             :::
        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        private double rad2deg(double rad)
        {
            return (rad / Math.PI * 180.0);
        }

        [HttpGet]
        [Route("api/Search/{businessType}/{postcode}")]
        public IHttpActionResult Search(string businessType = null, string postcode = null, double distance = 10)
        {
            try
            {



                string userId = null;

                try
                {
                    var user = User as ClaimsPrincipal;

                    if (user != null)
                    {
                        userId = user.Identity.GetUserId();
                    }

                }
                catch (Exception)
                {

                    //throw;
                }

                var
               applicationDbContext = db;

                var businessType2 = db.BusinessTypes.FirstOrDefault(type => type.Name == businessType);

                if (businessType2 == null)
                {
                    return Content(HttpStatusCode.NotFound, "The given business type could not be found in the database");
                }

                var searchLocation = PostcodesService.Lookup(postcode);



                //  SearchStore searchStore = new SearchStore(applicationDbContext);

                // searchStore.Search(postcode, businessType);



                double distanceInMiles = distance;

                double distanceInMeters = GeoUtils.MilesToMeters(distanceInMiles);


                var searchResults =
                    applicationDbContext.Businesses.Where(
                        t => t.BusinessTypeId == businessType2.Id && t.Location.Distance(searchLocation) <= distanceInMeters)
                        .OrderBy(business => business.Location.Distance(searchLocation));

                //var q = applicationDbContext.Businesses;


                SearchResponse searchResponse = new SearchResponse();

                searchResponse.BusinessType = businessType2.Name;
                searchResponse.BusinessTypeId = businessType2.Id;
                searchResponse.Postcode = postcode;



                searchResponse.Latitude = searchLocation.Latitude.Value;
                searchResponse.Longitude = searchLocation.Longitude.Value;

                searchResponse.Within = distance;
                searchResponse.WithinM = distanceInMeters;

                List<BusinessSearchResult> results = new List<BusinessSearchResult>();

                foreach (Business business in searchResults)
                {

                    try
                    {
                        double distanceFromPostcode = 0;

                        double blat = searchResponse.Latitude;
                        double blong = searchResponse.Longitude;

                        try
                        {

                            distanceFromPostcode = business.Location.Distance(searchLocation).Value;

                            if (business != null)
                            {
                                if (business.Location != null)
                                {
                                    var loc = business.Location;

                                    blat = loc.Latitude.Value;
                                    blong = loc.Longitude.Value;
                                }
                            }
                        }
                        catch (Exception exception)
                        {
                            distanceFromPostcode = this. distance(searchResponse.Latitude, searchResponse.Longitude, blat,
                                blong, 'K') * 1000;

                            if (distanceFromPostcode > distanceInMeters)
                            {
                                continue;
                                
                            }
                        }

         
                        

                        var result = new BusinessSearchResult()
                        {
                            Distance = Math.Round(distanceFromPostcode, 0),
                            Name = business.Name,
                            BusinessId = business.Id,
                            Latitude = business.Location.Latitude,
                            Longitude = business.Location.Longitude,
                            Postcode = business.Postcode,
                            Address = business.Address,
                            Website = business.Website,

                        };

                        results.Add(result);
                    }
                    catch (Exception exception)
                    {
                        
                        // just exclude the result.
                    }

                }

                searchResponse.Results = results;

                return Ok(searchResponse);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }
    }
}