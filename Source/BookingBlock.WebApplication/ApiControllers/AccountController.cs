using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using BookingBlock.WebApplication.Models;
using MarkEmbling.PostcodesIO;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BookingBlock.WebApplication.ApiControllers
{
    [System.Web.Http.RoutePrefix("api/account")]
    public class AccountController : BaseApiController
    {
        [HttpPost, Route("register")]
        public async Task<IHttpActionResult> Register(RegisterAccountRequest registerAccountRequest)
        {
            if (registerAccountRequest != null)
            {
                if (ModelState.IsValid)
                {
                    ApplicationDbContext context = ApplicationDbContext.Create();

                    UserStore<ApplicationUser> userStore = new UserStore<ApplicationUser>(context);

                    ApplicationUserManager userManager = new ApplicationUserManager(userStore);

                    string password = registerAccountRequest.Password;

                    string address = string.Join(",\r\n",
                        new
                        {
                            registerAccountRequest.AddressLine1,
                            registerAccountRequest.AddressLine2,
                            registerAccountRequest.TownCity
                        });

                    ApplicationUser newApplicationUser = new ApplicationUser();

                    newApplicationUser.FirstName = registerAccountRequest.FirstName;
                    newApplicationUser.LastName = registerAccountRequest.LastName;

                    newApplicationUser.Address = address;
                    newApplicationUser.Postcode = registerAccountRequest.Postcode;

                    newApplicationUser.Email = registerAccountRequest.EmailAddress;
                    newApplicationUser.UserName = registerAccountRequest.EmailAddress;

                    PostcodesIOClient client = new PostcodesIOClient();

                    var postcodeLookup = client.Lookup(newApplicationUser.Postcode);

                    newApplicationUser.Location = GeoUtils.CreatePoint(postcodeLookup.Latitude, postcodeLookup.Latitude);


                    var result = await userManager.CreateAsync(newApplicationUser, password);

                    if (result.Succeeded)
                    {
                        return Ok();
                    }

                    return BadRequest(string.Join(", ", result.Errors));
                }

                string validationErrors = string.Join(",",
                    ModelState.Values.Where(E => E.Errors.Count > 0)
                    .SelectMany(E => E.Errors)
                    .Select(E => E.ErrorMessage)
                    .ToArray());

                return BadRequest(validationErrors);
            }

            return BadRequest("registration data is null.");
        }

        [HttpGet, Route("countries")]
        public async Task<IHttpActionResult> Countries()
        {
            CultureInfo[] specificCultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

            List< string > countries = new List<string>();

            foreach (CultureInfo specificCulture in specificCultures)
            {
                var lcid = specificCulture.LCID;

                RegionInfo regionInfo = new RegionInfo(lcid);

                if (!countries.Contains(regionInfo.EnglishName))
                {
                    countries.Add(regionInfo.EnglishName);
                }

               
            }

            await Task.Run(() => countries.Sort());

            return Ok(countries);
        } 
    }
}