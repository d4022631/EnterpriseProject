using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using BookingBlock.EntityFramework;
using BookingBlock.WebApplication.Models;

namespace BookingBlock.WebApplication.ApiControllers
{
    public class BusinessOpeningTimesController : BaseApiController
    {
   

        // GET: api/BusinessOpeningTimes
        public IQueryable<BusinessOpeningTime> GetBusinessOpeningTimes()
        {
            return db.BusinessOpeningTimes;
        }

        // GET: api/BusinessOpeningTimes/5
        [ResponseType(typeof(BusinessOpeningTime))]
        public async Task<IHttpActionResult> GetBusinessOpeningTime(Guid id)
        {
            BusinessOpeningTime businessOpeningTime =
                await db.BusinessOpeningTimes.FirstOrDefaultAsync(time => time.BusinessId == id);
            if (businessOpeningTime == null)
            {
                return NotFound();
            }

            return Ok(businessOpeningTime);
        }

        // PUT: api/BusinessOpeningTimes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutBusinessOpeningTime(Guid id, BusinessOpeningTime businessOpeningTime)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != businessOpeningTime.BusinessId)
            {
                return BadRequest();
            }

            db.Entry(businessOpeningTime).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BusinessOpeningTimeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/BusinessOpeningTimes
        [ResponseType(typeof(BusinessOpeningTime))]
        public async Task<IHttpActionResult> PostBusinessOpeningTime(BusinessOpeningTime businessOpeningTime)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.BusinessOpeningTimes.Add(businessOpeningTime);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (BusinessOpeningTimeExists(businessOpeningTime.BusinessId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = businessOpeningTime.BusinessId }, businessOpeningTime);
        }

        // DELETE: api/BusinessOpeningTimes/5
        [ResponseType(typeof(BusinessOpeningTime))]
        public async Task<IHttpActionResult> DeleteBusinessOpeningTime(Guid id)
        {
            BusinessOpeningTime businessOpeningTime = await db.BusinessOpeningTimes.FirstOrDefaultAsync(time => time.BusinessId == id);
            if (businessOpeningTime == null)
            {
                return NotFound();
            }

            db.BusinessOpeningTimes.Remove(businessOpeningTime);
            await db.SaveChangesAsync();

            return Ok(businessOpeningTime);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BusinessOpeningTimeExists(Guid id)
        {
            return db.BusinessOpeningTimes.Count(e => e.BusinessId == id) > 0;
        }
    }
}