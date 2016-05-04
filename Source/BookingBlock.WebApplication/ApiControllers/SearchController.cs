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
        [HttpGet]
        [Route("api/Search/{businessType}/{postcode}")]
        public IHttpActionResult Search(string businessType = null, string postcode = null, double distance = 10)
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

                double distanceFromPostcode = business.Location.Distance(searchLocation).Value;

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

            searchResponse.Results = results;

            return Ok(searchResponse);
        }
    }
}