using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using BookingBlock.EntityFramework;
using BookingBlock.WebApplication.Models;

namespace BookingBlock.WebApplication.ApiControllers
{
    public class BusinessServicesRequest
    {
        public Guid BusinessId { get; set; }

        public List<Service> Services { get; set; } = new List<Service>(); 
    }

    [RoutePrefix("api/services")]
    public class ServicesController : BaseApiController
    {
        [Route("{id}/list"), HttpGet]
        public IHttpActionResult Get(Guid id)
        {
            return Ok(db.Services.Where(service => service.BusinessId == id).ToList());
        }

        [Route("create"), HttpPost]
        public IHttpActionResult Add(AddServiceRequest addServiceRequest)
        {
            if (addServiceRequest == null)
            {
                return BadRequest("No service information was posted with the request.");
            }

            if (!ModelState.IsValid)
            {
                return InvalidModel();
            }

            if (!this.IsUserAuthenticated)
            {
                return Content(HttpStatusCode.Unauthorized, "You must be logged in to add a service.");
            }

            var business = db.Businesses.FirstOrDefault(business1 => business1.Id == addServiceRequest.BusinessId);

            if (business == null)
            {
                return Content(HttpStatusCode.NotFound,
                    $"A business with the given id {addServiceRequest.BusinessId} does not exist in the database");
            }

            bool hasPermission = db.BusinessUsers.Any(user => user.BusinessId == addServiceRequest.BusinessId && user.UserId == this.UserId);

            if (!hasPermission)
            {
                return Content(HttpStatusCode.Forbidden,
                    $"The user current user {UserId} does not have access to the business {addServiceRequest.BusinessId}.");
            }

            var s = new Service()
            {
                Cost = addServiceRequest.Cost,
                Description = addServiceRequest.Description,
                Duration = addServiceRequest.Duration,
                Name = addServiceRequest.Name
            };

            business.Services.Add(s);

            int result = db.SaveChanges();


            return Ok(s.Id);
        }
    }
}