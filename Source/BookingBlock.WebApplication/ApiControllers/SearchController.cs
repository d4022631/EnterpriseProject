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


            PostcodesIOClient client = new PostcodesIOClient();


            var p = client.Lookup(postcode);

            var searchLocation = GeoUtils.CreatePoint(p.Latitude, p.Longitude);



            //  SearchStore searchStore = new SearchStore(applicationDbContext);

            // searchStore.Search(postcode, businessType);



            double distanceInMiles = distance;

            double distanceInMeters = GeoUtils.MilesToMeters(distanceInMiles);


            //   var q = applicationDbContext.Businesses.Where(  t => t.BusinessTypeId == businessType2.Id && t.Location.Distance(searchLocation) < distanceInMeters).OrderBy(f => f.Location.Distance(searchLocation));
            var q = applicationDbContext.Businesses.Where(t => t.BusinessTypeId == businessType2.Id);


            SearchResponse searchResponse = new SearchResponse();

            searchResponse.BusinessType = businessType;
            searchResponse.Postcode = postcode;
            searchResponse.Latitude = p.Latitude;
            searchResponse.Longitude = p.Longitude;

            searchResponse.Within = distance;

            List<BusinessSearchResult> results = new List<BusinessSearchResult>();

            foreach (var business in q)
            {

                double distanceFromPostcode = business.Location.Distance(searchLocation).Value;

                var result = new BusinessSearchResult()
                {
                    Distance = GeoUtils.MetersToMiles(distanceFromPostcode),
                    Name = business.Name,
                    BusinessId = business.Id,
                    Latitude = business.Location.Latitude,
                    Longitude = business.Location.Longitude
                };

               results.Add(result);

            }

            searchResponse.Results = results;

            return Ok(searchResponse);
        }
    }
}