using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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

    public class ServiceViewModel
    {
        public Guid BusinessId { get; set; }
        public string BusinessName { get; set; }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public TimeSpan Duration { get; set; }

        public decimal Cost { get; set; }
    }

    [RoutePrefix("api/services")]
    public class ServicesController : BaseApiController
    {
        [Route("{id}/list"), HttpGet]
        public IHttpActionResult Get(Guid id)
        {
            return Ok(db.Services.Where(service => service.BusinessId == id).ToList());
        }

        [Route("list"), HttpGet]
        public IHttpActionResult All()
        {
            List<ServiceViewModel> services = new List<ServiceViewModel>();

            foreach (Business business in db.Businesses)
            {
                foreach (Service service in business.Services)
                {
                    services.Add(new ServiceViewModel()
                    {
                        BusinessId = business.Id, BusinessName = business.Name, Cost = service.Cost, Duration = service.Duration, Description = service.Description, Name = service.Name, Id = service.Id
                    });
                }
            }

            return Ok(services);
        }

        [Route("get-name"), HttpGet]
        public async Task<IHttpActionResult> GetName(Guid serviceId)
        {
            ServiceStore serviceStore = new ServiceStore(this.db);

            var result = await serviceStore.GetNameAsync(serviceId);

            if (string.IsNullOrWhiteSpace(result))
            {
                return NotFound();
            }

            return Ok(result);
        }

        [Route("create"), HttpPost]
        public async Task<IHttpActionResult> Add(AddServiceRequest addServiceRequest)
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

            ServiceStore serviceStore = new ServiceStore(db);

            var result =
                await
                    serviceStore.CreateAsync(addServiceRequest.BusinessId, addServiceRequest.Name,
                        addServiceRequest.Description, addServiceRequest.Cost, addServiceRequest.Duration);


            return Ok(result.Id);
        }
    }
}