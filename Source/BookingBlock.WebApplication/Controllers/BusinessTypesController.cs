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
    public class BusinessTypesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: BusinessTypes
        public async Task<ActionResult> Index()
        {
            return View(await db.BusinessTypes.ToListAsync());
        }

        // GET: BusinessTypes/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BusinessType businessType = await db.BusinessTypes.FindAsync(id);
            if (businessType == null)
            {
                return HttpNotFound();
            }
            return View(businessType);
        }

        // GET: BusinessTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BusinessTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Created,Modified,Deleted")] BusinessType businessType)
        {
            if (ModelState.IsValid)
            {
                businessType.Id = Guid.NewGuid();
                db.BusinessTypes.Add(businessType);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(businessType);
        }

        // GET: BusinessTypes/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BusinessType businessType = await db.BusinessTypes.FindAsync(id);
            if (businessType == null)
            {
                return HttpNotFound();
            }
            return View(businessType);
        }

        // POST: BusinessTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Created,Modified,Deleted")] BusinessType businessType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(businessType).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(businessType);
        }

        // GET: BusinessTypes/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BusinessType businessType = await db.BusinessTypes.FindAsync(id);
            if (businessType == null)
            {
                return HttpNotFound();
            }
            return View(businessType);
        }

        // POST: BusinessTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            BusinessType businessType = await db.BusinessTypes.FindAsync(id);
            db.BusinessTypes.Remove(businessType);
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
