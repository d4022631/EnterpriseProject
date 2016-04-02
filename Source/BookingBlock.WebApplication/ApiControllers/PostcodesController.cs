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
        [System.Web.Http.Route("autocomplete")]
        [SwaggerResponse(HttpStatusCode.OK, "Postcode auto", typeof(PostcodeAutocompleteResponse))]
        public async Task<IHttpActionResult> Autocomplete(PostcodeAutocompleteRequest autocompleteRequest)
        {
            return Response(HttpStatusCode.OK, new PostcodeAutocompleteResponse());
        }

        [System.Web.Http.Route("validate")]
        public async Task<IHttpActionResult> Validate(PostcodeValidationRequest validationRequest)
        {
            return Ok();
        }
    }

    public abstract class ApiResponse
    {
        public string Message { get; set; }
    }

    public abstract class PostcodeRequest
    {
        public string Postcode { get; set; }
    }

    public abstract class PostcodeResponse : ApiResponse
    {
        public string Postcode { get; set; }
    }



    public class PostcodeValidationRequest : PostcodeRequest
    {
        
    }

    public class PostcodeValidationResponse : PostcodeResponse
    {
        public bool IsValid { get; set; }
    }

    public class PostcodeAutocompleteResponse : PostcodeResponse
    {
        public List<string> Suggestions { get; set; } 
    }

    public class PostcodeAutocompleteRequest : PostcodeRequest
    {
     
    }
}