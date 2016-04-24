﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using BookingBlock.EntityFramework;
using BookingBlock.WebApi;
using BookingBlock.WebApplication.Models;
using IdentityServer3.Core.Validation;
using MarkEmbling.PostcodesIO;
using Microsoft.AspNet.Identity;

namespace BookingBlock.WebApplication.ApiControllers
{
    public class CreateBookingRequest
    {
        [Required]
        public Guid ServiceId { get; set; }

        [Required]
        public DateTime DateTime { get; set; }
    }


    [System.Web.Http.RoutePrefix("api/businesses")]
    public class BusinessesController : BaseApiController
    {

        [Route("change-type"), HttpPost]
        public async Task<IHttpActionResult> ChangeType(ChangeBusinessTypeRequest changeBusinessTypeRequestRequest)
        {
            // if the user is null or the user is not authenticated
            if (!IsUserAuthenticated)
            {
                return Content(HttpStatusCode.Unauthorized, "User must be logged to get businesses.");
            }

            var ownerId = this.UserId;

            var myBusinesses =
                db.BusinessUsers.Where(businessUser => businessUser.UserId == ownerId && businessUser.BusinessId == changeBusinessTypeRequestRequest.BusinessId)
                    .Include(businessUser => businessUser.Business).FirstOrDefault();

            var newBusiness = myBusinesses.Business;

            var firstOrDefault = db.BusinessTypes.FirstOrDefault(type => type.Name == changeBusinessTypeRequestRequest.Type);

            if (firstOrDefault != null)
                newBusiness.BusinessTypeId =
                    firstOrDefault.Id;

            db.SaveChanges();

            return Ok();
        }

        [Route("change-name"), HttpPost]
        public async Task<IHttpActionResult> ChangeName(ChangeBusinessNameRequest changeBusinessNameRequest)
        {
            // if the user is null or the user is not authenticated
            if (!IsUserAuthenticated)
            {
                return Content(HttpStatusCode.Unauthorized, "User must be logged to get businesses.");
            }

            var ownerId = this.UserId;

            var myBusinesses =
                db.BusinessUsers.Where(businessUser => businessUser.UserId == ownerId && businessUser.BusinessId == changeBusinessNameRequest.BusinessId)
                    .Include(businessUser => businessUser.Business).FirstOrDefault();

            var newBusiness = myBusinesses.Business;

            newBusiness.Name = changeBusinessNameRequest.Name;

            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Used to change the address of a business.
        /// </summary>
        /// <param name="changeBusinessAddressRequest"></param>
        /// <returns></returns>
        [Route("change-address"), HttpPost]
        public async Task<IHttpActionResult> ChangeAddress(ChangeBusinessAddressRequest changeBusinessAddressRequest)
        {
            // if the user is null or the user is not authenticated
            if (!IsUserAuthenticated)
            {
                return Content(HttpStatusCode.Unauthorized, "User must be logged to get businesses.");
            }

            var ownerId = this.UserId;

            var myBusinesses =
                db.BusinessUsers.Where(businessUser => businessUser.UserId == ownerId && businessUser.BusinessId == changeBusinessAddressRequest.BusinessId)
                    .Include(businessUser => businessUser.Business).FirstOrDefault();

            var newBusiness = myBusinesses.Business;

            newBusiness.Address = changeBusinessAddressRequest.GetAddressString();

            newBusiness.Postcode = changeBusinessAddressRequest.Postcode;

            newBusiness.Location = PostcodeDbGeography.Lookup(changeBusinessAddressRequest.Postcode);


            db.SaveChanges();

            return Ok();
        }

        [Route("my-businesses"), HttpGet]
        public async Task<IHttpActionResult> MyBusinesses()
        {
            // if the user is null or the user is not authenticated
            if (!IsUserAuthenticated)
            {
                return Content(HttpStatusCode.Unauthorized, "User must be logged to get businesses.");
            }

            var ownerId = this.UserId;

            var myBusinesses =
                db.BusinessUsers.Where(businessUser => businessUser.UserId == ownerId)
                    .Include(businessUser => businessUser.Business);


            UserBusinessInfoList businessInfoList = new UserBusinessInfoList();


            foreach (BusinessUser businessUser in myBusinesses)
            {
               var ul = businessUser.UserLevel;
                var jk = businessUser.Business.Name;
                var id = businessUser.BusinessId;

                businessInfoList.Add(new UserBusinessInfo() {Id = id, Name = jk, IsOwner = ul == BusinessUserLevel.Owner});
            }

            return Ok(businessInfoList);
        }

        private string[] RA()
        {
            string root = HttpContext.Current.Server.MapPath("~/Content/data");

            string[] files = Directory.GetFiles(root);

            string file = files.PickRandom();

            List<string[]> dataList = new List<string[]>();

            using (var ff = File.OpenRead(file))
            {
                using (StreamReader streamReader = new StreamReader(ff))
                {
                    int lineCount = 0;

                    while (true)
                    {
                        var t = streamReader.ReadLine();

                        if (t == null)
                        {
                            break;
                        }

                        if (lineCount > 0)
                        {
                            if (!string.IsNullOrWhiteSpace(t))
                            {
                                string[] columns = t.Split(',');

                                dataList.Add(columns);
                            }
                        }

                        lineCount++;
                    }
                }
            }

            var radom = dataList.PickRandom();

            return radom;
        }

        [Route("regster"), HttpPost]
        public async Task<IHttpActionResult> Register(BusinessRegistrationData businessRegistrationData)
        {

            // if the user is null or the user is not authenticated
            if (!IsUserAuthenticated)
            {
                return Content(HttpStatusCode.Unauthorized, "User must be logged in to create a business.");
            }


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

                newBusiness.Address = businessRegistrationData.GetAddressString();

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

                newBusiness.PhoneNumber = businessRegistrationData.ContactNumber;
                newBusiness.FaxNumber = businessRegistrationData.ContactFax;
                

                if (!string.IsNullOrWhiteSpace(businessRegistrationData.OwnerEmailAddress))
                {
                    // if not an administrator return a 403 error

                    ApplicationUserStore applicationUserStore = new ApplicationUserStore(db);

                    var applicationUser =
                        await applicationUserStore.FindByEmailAsync(businessRegistrationData.OwnerEmailAddress);

                    ownerId = applicationUser.Id;
                }
                else
                {
        
                    if (!IsUserAuthenticated)
                    {
                        return BadRequest("user bad");
                    }

                    ownerId = this.UserId;
                }
                newBusiness.Users.Add(new BusinessUser() { UserId = ownerId, UserLevel = BusinessUserLevel.Owner });

                db.Businesses.Add(newBusiness);

                await db.SaveChangesAsync();

                return Ok(ownerId);

            }

            return InvalidModel();
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

           

            // random business type.
            BusinessType businessType = db.BusinessTypes.OrderBy(r => Guid.NewGuid()).FirstOrDefault();

            // assign the business type id.
            business.BusinessTypeId = businessType.Id;

            var owner = db.Users.Where(user => user.IsDummy).OrderBy(r => Guid.NewGuid()).FirstOrDefault();

            business.Name = owner.LastName + "'s " + businessType.Name;

            business.OpeningTimes = new List<BusinessOpeningTime>();
            business.OpeningTimes.Add(new BusinessOpeningTime() { DayOfWeek = DayOfWeek.Monday, OpeningTime = TimeSpan.FromHours(8), ClosingTime = TimeSpan.FromHours(17)});
            business.OpeningTimes.Add(new BusinessOpeningTime() { DayOfWeek = DayOfWeek.Tuesday, OpeningTime = TimeSpan.FromHours(8), ClosingTime = TimeSpan.FromHours(17) });
            business.OpeningTimes.Add(new BusinessOpeningTime() { DayOfWeek = DayOfWeek.Wednesday, OpeningTime = TimeSpan.FromHours(8), ClosingTime = TimeSpan.FromHours(17) });
            business.OpeningTimes.Add(new BusinessOpeningTime() { DayOfWeek = DayOfWeek.Thursday, OpeningTime = TimeSpan.FromHours(8), ClosingTime = TimeSpan.FromHours(17) });
            business.OpeningTimes.Add(new BusinessOpeningTime() { DayOfWeek = DayOfWeek.Friday, OpeningTime = TimeSpan.FromHours(8), ClosingTime = TimeSpan.FromHours(17) });
            business.OpeningTimes.Add(new BusinessOpeningTime() { DayOfWeek = DayOfWeek.Saturday, OpeningTime = TimeSpan.FromHours(8), ClosingTime = TimeSpan.FromHours(17) });
            business.OpeningTimes.Add(new BusinessOpeningTime() { DayOfWeek = DayOfWeek.Sunday, OpeningTime = TimeSpan.FromHours(10), ClosingTime = TimeSpan.FromHours(16) });

            //something.OrderBy(r => Guid.NewGuid()).Take(5)

            var d = RA();
            business.Users.Add(new BusinessUser() {UserId = owner.Id, UserLevel = BusinessUserLevel.Owner});
            business.Postcode = d[9];
            business.Address = string.Join(",\r\n", d);
            business.IsDummy = true;

            PostcodesIOClient client = new PostcodesIOClient();

            var postcodeLookup = client.Lookup(business.Postcode);

            business.Location = GeoUtils.CreatePoint(postcodeLookup.Latitude, postcodeLookup.Latitude);

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
        public IEnumerable<Business> GetBusinesses()
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

        [Route("{id}/services"), HttpGet]
        public async Task<IHttpActionResult> GetServices(Guid id)
        {
            var services = db.Services.Where(service => service.BusinessId == id);


            return
                Ok(
                    services.Select(
                        service =>
                            new BusinessService()
                            {
                                Cost = service.Cost,
                                Duration = service.Duration,
                                Description = service.Description,
                                Name = service.Description,
                                ServiceId = service.Id
                            }));
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

    public class BusinessService
    {
        public Guid ServiceId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public TimeSpan Duration { get; set; }

        public decimal Cost { get; set; }
    }
}