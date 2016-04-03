using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using MarkEmbling.PostcodesIO;
using Swashbuckle.Swagger.Annotations;

namespace BookingBlock.WebApplication.ApiControllers
{
    public static class PostcodesIOClientAsyncExtensions
    {
        public static async Task<IEnumerable<string>> AutocompleteAsync(this PostcodesIOClient client, string postcode)
        {
            return await Task.Run(() => client.Autocomplete(postcode));
        }

        public static async Task<bool> ValidateAsync(this PostcodesIOClient client, string postcode)
        {
            return await Task.Run(() => client.Validate(postcode));
        } 
    }


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
                PostcodesIOClient client = new PostcodesIOClient();

                var result = await client.AutocompleteAsync(postcode);

                return Ok(result);
            }

            return BadRequest("the postcode code given in null or just contains white space characters.");
        }

        [System.Web.Http.Route("validate")]
        public async Task<IHttpActionResult> Validate(string postcode)
        {
            if (!string.IsNullOrWhiteSpace(postcode))
            {
                MarkEmbling.PostcodesIO.PostcodesIOClient client = new PostcodesIOClient();

                var result = await client.ValidateAsync(postcode);

                return Ok(result);
            }

            return BadRequest("the postcode code given in null or just contains white space characters.");


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