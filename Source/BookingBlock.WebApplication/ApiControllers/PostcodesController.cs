using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Swashbuckle.Swagger.Annotations;

namespace BookingBlock.WebApplication.ApiControllers
{
    [System.Web.Http.RoutePrefix("api/postcodes")]
    public class PostcodesController : BaseApiController
    {
        [System.Web.Http.HttpGet, System.Web.Http.Route("autocomplete/{postcode}")]
        [SwaggerResponse(HttpStatusCode.OK, "Postcode auto", typeof(IEnumerable<string>))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "The reponse body will contain a message detailing the error.")]
        public async Task<IHttpActionResult> Autocomplete(string postcode)
        {
            if (!string.IsNullOrWhiteSpace(postcode))
            {
                var result = await PostcodesService.AutoCompleteAsync(postcode);

                return Ok(result);
            }

            return BadRequest("the postcode code given in null or just contains white space characters.");
        }

        [System.Web.Http.Route("validate")]
        public async Task<IHttpActionResult> Validate(string postcode)
        {
            if (!string.IsNullOrWhiteSpace(postcode))
            {
                   var result = await PostcodesService.ValidateAsync(postcode);

                return Ok(result);
            }

            return BadRequest("the postcode code given in null or just contains white space characters.");


        }
    }
}