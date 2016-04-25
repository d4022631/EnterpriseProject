using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using BookingBlock.EntityFramework;
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

        [Route("business-types-distribution"), HttpGet]
        public async Task<IHttpActionResult> Bus(bool c = true)
        {
            BusinessTypesDistribution distribution = new BusinessTypesDistribution();

            var all = db.BusinessTypes.Include("Businesses").Select(
                type =>
                    new BusinessTypeDistribution()
                    {
                        BusinessTypeId = type.Id,
                        BusinessTypeName = type.Name,
                        Count = type.Businesses.Count
                    });

            if (c)
            {
                distribution.AddRange(all.Where(typeDistribution => typeDistribution.Count > 0));
            }
            else
            {
                distribution.AddRange(all);
            }

           

            return Ok(distribution);
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
}