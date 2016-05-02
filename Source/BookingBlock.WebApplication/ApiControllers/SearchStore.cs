using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using BookingBlock.EntityFramework;
using BookingBlock.WebApplication.Models;
using MarkEmbling.PostcodesIO;
using MarkEmbling.PostcodesIO.Results;

namespace BookingBlock.WebApplication.ApiControllers
{
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

            DbGeography postcodeLocation = PostcodesService.Lookup(postcode);

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
}