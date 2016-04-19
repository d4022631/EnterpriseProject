using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using BookingBlock.WebApplication.Models;

namespace BookingBlock.WebApplication.ApiControllers
{
    [System.Web.Http.RoutePrefix("api/business-intelligence")]
    public class BusinessIntelligenceController : BaseApiController
    {
        [Route("age-distribution"), HttpGet]
        public async Task<IHttpActionResult> RandomAccount()
        {
            AgeDistribution ageDistribution = new AgeDistribution();

            ApplicationDbContext context = ApplicationDbContext.Create();

            foreach (ApplicationUser applicationUser in context.Users)
            {
                int age = DateTime.Now.Year - applicationUser.DateOfBirth.Year;
                if (ageDistribution.Ages.ContainsKey(age))
                {
                    ageDistribution.Ages[age]++;
                }
                else
                {
                    ageDistribution.Ages.Add(age, 1);
                }
            }


            return Ok(ageDistribution);
        }

        [Route("gender-distribution"), HttpGet]
        public async Task<IHttpActionResult> GenderDistribution()
        {
            GenderDistribution genderDistribution = new GenderDistribution();

            ApplicationDbContext context = ApplicationDbContext.Create();

            foreach (ApplicationUser applicationUser in context.Users)
            {
                if (applicationUser.Gender == Gender.Female)
                {
                    genderDistribution.Females++;
                }
                else
                {
                    genderDistribution.Males++;
                }
            }


            return Ok(genderDistribution);
        }
    }

    public class AgeDistribution
    {
        public Dictionary<int, int> Ages { get; set; } = new Dictionary<int, int>();
    }

    public class GenderDistribution
    {
        public int Males { get; set; }

        public int Females { get; set; }
    }
}