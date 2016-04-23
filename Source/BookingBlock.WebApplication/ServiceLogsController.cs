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

namespace BookingBlock.WebApplication
{
    public class ServiceLogsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ServiceLogs
        public async Task<ActionResult> Index()
        {
            var serviceLogs = db.ServiceLogs.Include(s => s.Service);
            return View(await serviceLogs.ToListAsync());
        }

        // GET: ServiceLogs/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceLog serviceLog = await db.ServiceLogs.FindAsync(id);
            if (serviceLog == null)
            {
                return HttpNotFound();
            }
            return View(serviceLog);
        }

        // GET: ServiceLogs/Create
        public ActionResult Create()
        {
            ViewBag.SeviceId = new SelectList(db.Services, "Id", "Name");
            return View();
        }

        // POST: ServiceLogs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,SeviceId,EntryDateTime,Entry")] ServiceLog serviceLog)
        {
            if (ModelState.IsValid)
            {
                serviceLog.Id = Guid.NewGuid();
                db.ServiceLogs.Add(serviceLog);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.SeviceId = new SelectList(db.Services, "Id", "Name", serviceLog.SeviceId);
            return View(serviceLog);
        }

        // GET: ServiceLogs/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceLog serviceLog = await db.ServiceLogs.FindAsync(id);
            if (serviceLog == null)
            {
                return HttpNotFound();
            }
            ViewBag.SeviceId = new SelectList(db.Services, "Id", "Name", serviceLog.SeviceId);
            return View(serviceLog);
        }

        // POST: ServiceLogs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,SeviceId,EntryDateTime,Entry")] ServiceLog serviceLog)
        {
            if (ModelState.IsValid)
            {
                db.Entry(serviceLog).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.SeviceId = new SelectList(db.Services, "Id", "Name", serviceLog.SeviceId);
            return View(serviceLog);
        }

        // GET: ServiceLogs/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceLog serviceLog = await db.ServiceLogs.FindAsync(id);
            if (serviceLog == null)
            {
                return HttpNotFound();
            }
            return View(serviceLog);
        }

        // POST: ServiceLogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            ServiceLog serviceLog = await db.ServiceLogs.FindAsync(id);
            db.ServiceLogs.Remove(serviceLog);
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
