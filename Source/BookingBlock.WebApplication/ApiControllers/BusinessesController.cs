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
using BookingBlock.WebApplication.Models;

namespace BookingBlock.WebApplication.ApiControllers
{
    public class BusinessesController : BaseApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Businesses
        public IQueryable<Business> GetBusinesses()
        {
            return db.Businesses;
        }

        // GET: api/Businesses/5
        [ResponseType(typeof(Business))]
        public async Task<IHttpActionResult> GetBusiness(Guid id)
        {
            Business business = await db.Businesses.FindAsync(id);
            if (business == null)
            {
                return NotFound();
            }

            return Ok(business);
        }

        // PUT: api/Businesses/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutBusiness(Guid id, Business business)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != business.Id)
            {
                return BadRequest();
            }

            db.Entry(business).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BusinessExists(id))
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

        // POST: api/Businesses
        [ResponseType(typeof(Business))]
        public async Task<IHttpActionResult> PostBusiness(Business business)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Businesses.Add(business);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = business.Id }, business);
        }

        // DELETE: api/Businesses/5
        [ResponseType(typeof(Business))]
        public async Task<IHttpActionResult> DeleteBusiness(Guid id)
        {
            Business business = await db.Businesses.FindAsync(id);
            if (business == null)
            {
                return NotFound();
            }

            db.Businesses.Remove(business);
            await db.SaveChangesAsync();

            return Ok(business);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BusinessExists(Guid id)
        {
            return db.Businesses.Count(e => e.Id == id) > 0;
        }
    }
}