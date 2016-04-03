using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using BookingBlock.WebApplication.Models;
using MarkEmbling.PostcodesIO;
using Microsoft.AspNet.Identity;

namespace BookingBlock.WebApplication.ApiControllers
{
    public class SearchController : ApiController
    {
        [HttpGet]
        [Route("api/Search/{businessType}/{postcode}")]
        public IHttpActionResult Search(string businessType = null, string postcode = null)
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

            var c = ApplicationDbContext.Create();

          PostcodesIOClient client = new PostcodesIOClient();

            var p = client.Lookup(postcode);

            var l = GeoUtils.CreatePoint(p.Latitude, p.Longitude);

            var q = c.Businesses.Where(t => t.Location.Distance(l) < 1000).OrderBy(f => f.Location.Distance(l));


            SearchResponse searchResponse = new SearchResponse();

            searchResponse.BusinessType = businessType;
            searchResponse.Postcode = postcode;
            searchResponse.Within = 10;

            List<BusinessSearchResult> results = new List<BusinessSearchResult>();

            foreach (var business in q)
            {

               results.Add(new BusinessSearchResult() { Distance = business.Location.Distance(l).Value, Name = business.Name });

            }

            searchResponse.Results = results;

            return Ok(searchResponse);
        }
    }
}