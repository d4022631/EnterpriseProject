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
    public class ServicesController : Controller
    { 
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Services
        public async Task<ActionResult> Index()
        {
            var services = db.Services.Include(s => s.Business);
            return View(await services.ToListAsync());
        }

        // GET: Services/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = await db.Services.FindAsync(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            return View(service);
        }

        // GET: Services/Create
        public ActionResult Create()
        {
            ViewBag.BusinessId = new SelectList(db.Businesses, "Id", "Name");
            return View();
        }

        // POST: Services/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,BusinessId,Name,Description,Cost,Duration")] Service service)
        {
            if (ModelState.IsValid)
            {
                service.Id = Guid.NewGuid();
                db.Services.Add(service);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.BusinessId = new SelectList(db.Businesses, "Id", "Name", service.BusinessId);
            return View(service);
        }

        // GET: Services/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = await db.Services.FindAsync(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            ViewBag.BusinessId = new SelectList(db.Businesses, "Id", "Name", service.BusinessId);
            return View(service);
        }

        // POST: Services/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,BusinessId,Name,Description,Cost,Duration")] Service service)
        {
            if (ModelState.IsValid)
            {
                db.Entry(service).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.BusinessId = new SelectList(db.Businesses, "Id", "Name", service.BusinessId);
            return View(service);
        }

        // GET: Services/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = await db.Services.FindAsync(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            return View(service);
        }

        // POST: Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            Service service = await db.Services.FindAsync(id);
            db.Services.Remove(service);
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
