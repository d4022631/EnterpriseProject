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
    public class BusinessUsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: BusinessUsers
        public async Task<ActionResult> Index()
        {
            var businessUsers = db.BusinessUsers.Include(b => b.Business).Include(b => b.User);
            return View(await businessUsers.ToListAsync());
        }

        // GET: BusinessUsers/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BusinessUser businessUser = await db.BusinessUsers.FindAsync(id);
            if (businessUser == null)
            {
                return HttpNotFound();
            }
            return View(businessUser);
        }

        // GET: BusinessUsers/Create
        public ActionResult Create()
        {
            ViewBag.BusinessId = new SelectList(db.Businesses, "Id", "Name");
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        // POST: BusinessUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "BusinessId,UserId,UserLevel")] BusinessUser businessUser)
        {
            if (ModelState.IsValid)
            {
            
                db.BusinessUsers.Add(businessUser);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.BusinessId = new SelectList(db.Businesses, "Id", "Name", businessUser.BusinessId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", businessUser.UserId);
            return View(businessUser);
        }

        // GET: BusinessUsers/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BusinessUser businessUser = await db.BusinessUsers.FindAsync(id);
            if (businessUser == null)
            {
                return HttpNotFound();
            }
            ViewBag.BusinessId = new SelectList(db.Businesses, "Id", "Name", businessUser.BusinessId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", businessUser.UserId);
            return View(businessUser);
        }

        // POST: BusinessUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "BusinessId,UserId,UserLevel")] BusinessUser businessUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(businessUser).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.BusinessId = new SelectList(db.Businesses, "Id", "Name", businessUser.BusinessId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", businessUser.UserId);
            return View(businessUser);
        }

        // GET: BusinessUsers/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BusinessUser businessUser = await db.BusinessUsers.FindAsync(id);
            if (businessUser == null)
            {
                return HttpNotFound();
            }
            return View(businessUser);
        }

        // POST: BusinessUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            BusinessUser businessUser = await db.BusinessUsers.FindAsync(id);
            db.BusinessUsers.Remove(businessUser);
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
