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
using BookingBlock.WebApplication.Models;

namespace BookingBlock.WebApplication.Controllers
{
    public class BusinessOpeningTimesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: BusinessOpeningTimes
        public async Task<ActionResult> Index()
        {
            var businessOpeningTimes = db.BusinessOpeningTimes.Include(b => b.Business);
            return View(await businessOpeningTimes.ToListAsync());
        }

        // GET: BusinessOpeningTimes/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BusinessOpeningTime businessOpeningTime = await db.BusinessOpeningTimes.FindAsync(id);
            if (businessOpeningTime == null)
            {
                return HttpNotFound();
            }
            return View(businessOpeningTime);
        }

        // GET: BusinessOpeningTimes/Create
        public ActionResult Create()
        {
            ViewBag.BusinessId = new SelectList(db.Businesses, "Id", "Name");
            return View();
        }

        // POST: BusinessOpeningTimes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "BusinessId,DayOfWeek,OpeningTime,ClosingTime")] BusinessOpeningTime businessOpeningTime)
        {
            if (ModelState.IsValid)
            {
                db.BusinessOpeningTimes.Add(businessOpeningTime);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.BusinessId = new SelectList(db.Businesses, "Id", "Name", businessOpeningTime.BusinessId);
            return View(businessOpeningTime);
        }

        // GET: BusinessOpeningTimes/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BusinessOpeningTime businessOpeningTime = await db.BusinessOpeningTimes.FindAsync(id);
            if (businessOpeningTime == null)
            {
                return HttpNotFound();
            }
            ViewBag.BusinessId = new SelectList(db.Businesses, "Id", "Name", businessOpeningTime.BusinessId);
            return View(businessOpeningTime);
        }

        // POST: BusinessOpeningTimes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "BusinessId,DayOfWeek,OpeningTime,ClosingTime")] BusinessOpeningTime businessOpeningTime)
        {
            if (ModelState.IsValid)
            {
                db.Entry(businessOpeningTime).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.BusinessId = new SelectList(db.Businesses, "Id", "Name", businessOpeningTime.BusinessId);
            return View(businessOpeningTime);
        }

        // GET: BusinessOpeningTimes/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BusinessOpeningTime businessOpeningTime = await db.BusinessOpeningTimes.FindAsync(id);
            if (businessOpeningTime == null)
            {
                return HttpNotFound();
            }
            return View(businessOpeningTime);
        }

        // POST: BusinessOpeningTimes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            BusinessOpeningTime businessOpeningTime = await db.BusinessOpeningTimes.FindAsync(id);
            db.BusinessOpeningTimes.Remove(businessOpeningTime);
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
