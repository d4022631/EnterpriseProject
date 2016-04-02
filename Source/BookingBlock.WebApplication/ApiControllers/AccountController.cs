using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace BookingBlock.WebApplication.ApiControllers
{
 

    public class AccountController : BaseApiController
    {
        [HttpGet, Route("api/account/countries")]
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