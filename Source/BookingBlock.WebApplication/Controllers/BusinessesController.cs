using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BookingBlock.EntityFramework;

namespace BookingBlock.WebApplication.Controllers
{
    public class BusinessesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Businesses
        public async Task<ActionResult> Index()
        {
            var businesses = db.Businesses.Include(b => b.BusinessType);
            return View(await businesses.ToListAsync());
        }

        // GET: Businesses/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Business business = await db.Businesses.FindAsync(id);
            if (business == null)
            {
                return HttpNotFound();
            }
            return View(business);
        }

        // GET: Businesses/Create
        public ActionResult Create()
        {
            ViewBag.BusinessTypeId = new SelectList(db.BusinessTypes, "Id", "Name");
            return View();
        }

        // POST: Businesses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Address,Postcode,PhoneNumber,FaxNumber,Website,Facebook,LinkedIn,GooglePlus,RegistrationDate,EmailAddress,LogoUrl,Location,BusinessTypeId,Created,Modified,Deleted")] Business business)
        {
            if (ModelState.IsValid)
            {
                business.Id = Guid.NewGuid();
                db.Businesses.Add(business);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.BusinessTypeId = new SelectList(db.BusinessTypes, "Id", "Name", business.BusinessTypeId);
            return View(business);
        }

        // GET: Businesses/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Business business = await db.Businesses.FindAsync(id);
            if (business == null)
            {
                return HttpNotFound();
            }
            ViewBag.BusinessTypeId = new SelectList(db.BusinessTypes, "Id", "Name", business.BusinessTypeId);
            return View(business);
        }

        // POST: Businesses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Address,Postcode,PhoneNumber,FaxNumber,Website,Facebook,LinkedIn,GooglePlus,RegistrationDate,EmailAddress,LogoUrl,Location,BusinessTypeId,Created,Modified,Deleted")] Business business)
        {
            if (ModelState.IsValid)
            {
                db.Entry(business).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.BusinessTypeId = new SelectList(db.BusinessTypes, "Id", "Name", business.BusinessTypeId);
            return View(business);
        }

        // GET: Businesses/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Business business = await db.Businesses.FindAsync(id);
            if (business == null)
            {
                return HttpNotFound();
            }
            return View(business);
        }

        // POST: Businesses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            Business business = await db.Businesses.FindAsync(id);
            db.Businesses.Remove(business);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
