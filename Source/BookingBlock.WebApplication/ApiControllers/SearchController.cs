using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using BookingBlock.WebApplication.Models;
using MarkEmbling.PostcodesIO;
using MarkEmbling.PostcodesIO.Results;
using Microsoft.AspNet.Identity;

namespace BookingBlock.WebApplication.ApiControllers
{
    public abstract class ApplicationDbContextStore
    {
        protected ApplicationDbContext _context;

        protected ApplicationDbContextStore(ApplicationDbContext context)
        {
            this._context = context;
        }
    }

    public sealed class BusinessTypeStore : ApplicationDbContextStore
    {
        public BusinessTypeStore(ApplicationDbContext context) : base(context)
        {
        }

        public BusinessType FindByName(string name)
        {
            return _context.BusinessTypes.FirstOrDefault(type => type.Name == name);
        }

        public async Task<BusinessType> FindByNameAsync(string name)
        {
            return await _context.BusinessTypes.FirstOrDefaultAsync(type => type.Name == name);
        } 
    }

    public sealed class SearchStore : ApplicationDbContextStore
    {
        private BusinessTypeStore _businessTypeStore;

        public SearchStore(ApplicationDbContext context) : base(context)
        {
            _businessTypeStore = new BusinessTypeStore(context);
        }

        private double FixDistance(double distance)
        {
            if (double.IsNaN(distance) || double.IsInfinity(distance))
            {
                return 10.0;
            }

            if (distance < 1.0)
            {
                return 1.0;
            }

            if (distance > 100)
            {
                return 100;
            }

            return distance;
        }

        public IEnumerable<Business> Search(string postcode, string type, double distance = 10.0, bool ascending = true, int page = 1,
            int pageSize = 25)
        {
            double fixedDistance = FixDistance(distance);
            int fixedPage = page < 1 ? 1 : page;
            int fixedPageSize = pageSize < 10 ? 10 : pageSize;

            PostcodesIOClient postcodesIoClient = new PostcodesIOClient();

            PostcodeResult postcodeLookupResult = postcodesIoClient.Lookup(postcode);

            DbGeography postcodeLocation = GeoUtils.CreatePoint(postcodeLookupResult.Latitude, postcodeLookupResult.Longitude);

            BusinessType businessType = _businessTypeStore.FindByName(type);

            double distanceInMeters = GeoUtils.MilesToMeters(fixedDistance);

            int skip = (fixedPage - 1)*fixedPageSize;

            var searchFilter = _context.Businesses.Where(t => t.Location.Distance(postcodeLocation) <= distanceInMeters && t.BusinessTypeId == businessType.Id);

            if (ascending)
            {
                return searchFilter.OrderBy(f => f.Location.Distance(postcodeLocation)).Skip(skip).Take(fixedPageSize);
            }
            else
            {
                return searchFilter.OrderByDescending(f => f.Location.Distance(postcodeLocation)).Skip(skip).Take(fixedPageSize);
            }

           
        } 
    }


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

            var applicationDbContext = ApplicationDbContext.Create();

            SearchStore searchStore = new SearchStore(applicationDbContext);

            searchStore.Search(postcode, businessType);

            PostcodesIOClient client = new PostcodesIOClient();

            double distanceInMiles = 10;

            double distanceInMeters = GeoUtils.MilesToMeters(distanceInMiles);

            var p = client.Lookup(postcode);

            var l = GeoUtils.CreatePoint(p.Latitude, p.Longitude);

            var q = applicationDbContext.Businesses.Where(t => t.Location.Distance(l) < distanceInMeters).OrderBy(f => f.Location.Distance(l));


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