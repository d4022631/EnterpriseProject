using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web.Http;
using BookingBlock.WebApi;

namespace BookingBlock.WebApplication.ApiControllers
{
    public class IdentityController : BaseApiController
    {
        [Route("api/identity/claims")]
        public IHttpActionResult Get()
        {
            var r = Request;

            var user = User as ClaimsPrincipal;

            if (user != null)
            {

                var claims = from c in user.Claims
                    select new
                    {
                        type = c.Type,
                        value = c.Value
                    };

                return Json(claims);
            }

            // if we are here then the user object is null, this idicates the caller is not authenticated
            return StatusCode(HttpStatusCode.Unauthorized);
        }

        [Route("api/identity/who"), HttpGet]
        public IHttpActionResult WhoAmI()
        {
            if (IsUserAuthenticated)
            {
                return Ok(this.UserId);
            }

            return Ok(Guid.Empty.ToString());
        }


        [Route("api/identity/get-users")]
        public IHttpActionResult GetUsers()
        {
            var t = db.Users;



            return Ok(t.Select(user => new ApplicationUserInfo() {Email = user.Email, Id = user.Id, Username = user.UserName }));
        }
    }

}