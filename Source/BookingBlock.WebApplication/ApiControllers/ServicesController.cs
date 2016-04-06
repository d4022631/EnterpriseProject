using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Http;

namespace BookingBlock.WebApplication.ApiControllers
{
    [RoutePrefix("api/services")]
    public class ServicesController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Add(AddServiceRequest addServiceRequest)
        {
            return Ok();
        }
    }

    public class AddServiceRequest
    {
        [Required]
        public Guid BusinessId { get; set; }

        [Required]
        public string Name { get; set; }

        public decimal Cost { get; set; }

        [Required]
        public TimeSpan Duration { get; set; }
    }
}