using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using BookingBlock.WebApplication.Models;
using MarkEmbling.PostcodesIO;
using Microsoft.AspNet.Identity;

namespace BookingBlock.WebApplication.ApiControllers
{
    public static class PostcodeDbGeography
    {
        public static DbGeography Lookup(string postcode)
        {

            PostcodesIOClient client = new PostcodesIOClient();

            var postcodeLookup = client.Lookup(postcode);

            return GeoUtils.CreatePoint(postcodeLookup.Latitude, postcodeLookup.Latitude);
        }
    }

    [System.Web.Http.RoutePrefix("api/businesses")]
    public class BusinessesController : BaseApiController
    {
        private string CreateBusinessAddress(BusinessRegistrationData businessRegistrationData)
        {
            List<string> addressParts = new List<string>();

            addressParts.Add(businessRegistrationData.AddressLine1);
            addressParts.Add(businessRegistrationData.AddressLine2);
            addressParts.Add(businessRegistrationData.TownCity);
            addressParts.Add(businessRegistrationData.Postcode);
            addressParts.Add(businessRegistrationData.Country);

            return string.Join(",\r\n", addressParts);
        }

        [Route("regster"), HttpPost]
        public async Task<IHttpActionResult> Register(BusinessRegistrationData businessRegistrationData)
        {
            // check that the model is valid.
            if (ModelState.IsValid)
            {
                Business newBusiness = new Business();

                newBusiness.Name = businessRegistrationData.Name;

                var businessType = await db.BusinessTypes.FirstOrDefaultAsync(type => type.Name == businessRegistrationData.Type);

                if (businessType == null)
                {
                    return Content(HttpStatusCode.BadRequest,
                        $"The given business type {businessRegistrationData.Type} could not be found in the database");
                }

                // set the business type id.
                newBusiness.BusinessTypeId = businessType.Id;

                newBusiness.Address = CreateBusinessAddress(businessRegistrationData);

                newBusiness.Postcode = businessRegistrationData.Postcode;

                newBusiness.Location = PostcodeDbGeography.Lookup(businessRegistrationData.Postcode);

                AddBusinessOpeningTime(newBusiness, DayOfWeek.Monday, businessRegistrationData.OpeningTimeMonday,
                    businessRegistrationData.ClosingTimeMonday);

                AddBusinessOpeningTime(newBusiness, DayOfWeek.Tuesday, businessRegistrationData.OpeningTimeTuesday,
                    businessRegistrationData.ClosingTimeTuesday);

                AddBusinessOpeningTime(newBusiness, DayOfWeek.Wednesday, businessRegistrationData.OpeningTimeWednesday,
                    businessRegistrationData.ClosingTimeWednesday);

                AddBusinessOpeningTime(newBusiness, DayOfWeek.Thursday, businessRegistrationData.OpeningTimeThursday,
                    businessRegistrationData.ClosingTimeThursday);

                AddBusinessOpeningTime(newBusiness, DayOfWeek.Friday, businessRegistrationData.OpeningTimeFriday,
                    businessRegistrationData.ClosingTimeFriday);

                AddBusinessOpeningTime(newBusiness, DayOfWeek.Saturday, businessRegistrationData.OpeningTimeSaturday,
                    businessRegistrationData.ClosingTimeSaturday);

                AddBusinessOpeningTime(newBusiness, DayOfWeek.Sunday, businessRegistrationData.OpeningTimeSunday,
                    businessRegistrationData.ClosingTimeSunday);

                string ownerId = string.Empty;

                if (!string.IsNullOrWhiteSpace(businessRegistrationData.OwnerEmailAddress))
                {
                    ApplicationUserStore applicationUserStore = new ApplicationUserStore(db);

                    var applicationUser =
                        await applicationUserStore.FindByEmailAsync(businessRegistrationData.OwnerEmailAddress);

                    ownerId = applicationUser.Id;
                }
                else
                {
                    var user = User as ClaimsPrincipal;


                    ownerId = user.Identity.GetUserId();
                }
                newBusiness.Users.Add(new BusinessUser() { UserId = ownerId, UserLevel = BusinessUserLevel.Owner });

                db.Businesses.Add(newBusiness);

                await db.SaveChangesAsync();

                return Ok(ownerId);

            }

            string validationErrors = string.Join(",",
                ModelState.Values.Where(modelState => modelState.Errors.Count > 0)
                .SelectMany(E => E.Errors)
                .Select(E => E.ErrorMessage)
                .ToArray());

            return BadRequest(validationErrors);
        }

        private void AddBusinessOpeningTime(Business newBusiness, DayOfWeek dayOfWeek, TimeSpan? openingTime, TimeSpan? closingTime)
        {
            var businessOpeningTime = CreateBusinessOpeningTime(dayOfWeek, openingTime, closingTime);

            if (businessOpeningTime != null)
            {
                newBusiness.OpeningTimes.Add(businessOpeningTime);
            }
        }

        private BusinessOpeningTime CreateBusinessOpeningTime(DayOfWeek dayOfWeek, TimeSpan? openingTime, TimeSpan? closingTime)
        {
            if (openingTime.HasValue && closingTime.HasValue)
            {
                return new BusinessOpeningTime()
                {
                    DayOfWeek = dayOfWeek,
                    OpeningTime = openingTime.Value,
                    ClosingTime = closingTime.Value
                };
            }

            return null;
        }


        [Route("create-random-business"), HttpGet]
        public async Task<IHttpActionResult> CreateRandomBusiness()
        {
            Business business = new Business();

            business.Name = "RANDOM";

            // random business type.
            BusinessType businessType = db.BusinessTypes.OrderBy(r => Guid.NewGuid()).FirstOrDefault();

            // assign the business type id.
            business.BusinessTypeId = businessType.Id;

            var owner = db.Users.Where(user => user.IsDummy).OrderBy(r => Guid.NewGuid()).FirstOrDefault();
            business.OpeningTimes = new List<BusinessOpeningTime>();
            business.OpeningTimes.Add(new BusinessOpeningTime() { DayOfWeek = DayOfWeek.Monday, OpeningTime = TimeSpan.FromHours(8), ClosingTime = TimeSpan.FromHours(17)});
            business.OpeningTimes.Add(new BusinessOpeningTime() { DayOfWeek = DayOfWeek.Tuesday, OpeningTime = TimeSpan.FromHours(8), ClosingTime = TimeSpan.FromHours(17) });
            business.OpeningTimes.Add(new BusinessOpeningTime() { DayOfWeek = DayOfWeek.Wednesday, OpeningTime = TimeSpan.FromHours(8), ClosingTime = TimeSpan.FromHours(17) });
            business.OpeningTimes.Add(new BusinessOpeningTime() { DayOfWeek = DayOfWeek.Thursday, OpeningTime = TimeSpan.FromHours(8), ClosingTime = TimeSpan.FromHours(17) });
            business.OpeningTimes.Add(new BusinessOpeningTime() { DayOfWeek = DayOfWeek.Friday, OpeningTime = TimeSpan.FromHours(8), ClosingTime = TimeSpan.FromHours(17) });
            business.OpeningTimes.Add(new BusinessOpeningTime() { DayOfWeek = DayOfWeek.Saturday, OpeningTime = TimeSpan.FromHours(8), ClosingTime = TimeSpan.FromHours(17) });
            business.OpeningTimes.Add(new BusinessOpeningTime() { DayOfWeek = DayOfWeek.Sunday, OpeningTime = TimeSpan.FromHours(10), ClosingTime = TimeSpan.FromHours(16) });

            //something.OrderBy(r => Guid.NewGuid()).Take(5)

            business.Users.Add(new BusinessUser() {UserId = owner.Id, UserLevel = BusinessUserLevel.Owner});
            business.Postcode = "TS23 2qh";
            business.Address = "dummy";
            business.IsDummy = true;
            business.Location = GeoUtils.CreatePoint(0, 0);
            db.Businesses.Add(business);
     
            try
            {
     int a = await db.SaveChangesAsync();

            
            }
            catch (Exception exception)
            {
                
                throw;
            }
       

            return Ok();
        }

        // GET: api/Businesses
        public IQueryable<Business> GetBusinesses()
        {
            return db.Businesses;
        }

        // GET: api/Businesses/5
        [ResponseType(typeof(Business))]
        public async Task<IHttpActionResult> GetBusiness(Guid id)
        {
            Business business = await db.Businesses.FindAsync(id);
            if (business == null)
            {
                return NotFound();
            }

            return Ok(business);
        }

        // PUT: api/Businesses/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutBusiness(Guid id, Business business)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != business.Id)
            {
                return BadRequest();
            }

            db.Entry(business).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BusinessExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Businesses
        [ResponseType(typeof(Business))]
        public async Task<IHttpActionResult> PostBusiness(Business business)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Businesses.Add(business);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = business.Id }, business);
        }

        // DELETE: api/Businesses/5
        [ResponseType(typeof(Business))]
        public async Task<IHttpActionResult> DeleteBusiness(Guid id)
        {
            Business business = await db.Businesses.FindAsync(id);
            if (business == null)
            {
                return NotFound();
            }

            db.Businesses.Remove(business);
            await db.SaveChangesAsync();

            return Ok(business);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BusinessExists(Guid id)
        {
            return db.Businesses.Count(e => e.Id == id) > 0;
        }
    }
}