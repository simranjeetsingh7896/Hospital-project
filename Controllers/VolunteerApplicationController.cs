using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using team2Geraldton.Models;

namespace team2Geraldton.Controllers
{
    public class VolunteerApplicationController : Controller
    {
        private team2GeraldtonDbContext db = new team2GeraldtonDbContext();


        // Only Admin can view volunteers list
        [Authorize(Roles = "Admin")]
        // GET: VolunteerApplication
        public ActionResult Index()
        {
            var volunteerApplications = db.VolunteerApplications.Include(v => v.VolunteerOpportunity);
            return View(volunteerApplications.ToList());
        }

        // GET: VolunteerApplication/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VolunteerApplication volunteerApplication = db.VolunteerApplications.Find(id);
            if (volunteerApplication == null)
            {
                return HttpNotFound();
            }
            return View(volunteerApplication);
        }

        // Only User can apply for the volunteer application and once user submit they will get a success message
        // GET: VolunteerApplication/Create
       // [Authorize(Roles = "User,Admin")]
        public ActionResult Create()
        {
            ViewBag.OpportunityID = new SelectList(db.VolunteerOpportunities, "OpportunityID", "OpportunityName");
            return View();
        }

        // Only User can apply for the volunteer application and once user submit they will get a success message
        // POST: VolunteerApplication/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "User,Admin")]
        public ActionResult Create([Bind(Include = "VolunteerID,Name,Address,City,Email,ContactNumber,Language,WhyInterested,PastVolunteer,OpportunityID")] VolunteerApplication volunteerApplication)
        {
            if (ModelState.IsValid)
            {
                db.VolunteerApplications.Add(volunteerApplication);
                db.SaveChanges();
                return RedirectToAction("Success");
            }

            ViewBag.OpportunityID = new SelectList(db.VolunteerOpportunities, "OpportunityID", "OpportunityName", volunteerApplication.OpportunityID);
            return View(volunteerApplication);
        }

        // Only admin can edit volunteers 
        [Authorize(Roles = "Admin")]
        // GET: VolunteerApplication/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VolunteerApplication volunteerApplication = db.VolunteerApplications.Find(id);
            if (volunteerApplication == null)
            {
                return HttpNotFound();
            }
            ViewBag.OpportunityID = new SelectList(db.VolunteerOpportunities, "OpportunityID", "OpportunityName", volunteerApplication.OpportunityID);
            return View(volunteerApplication);
        }

        // POST: VolunteerApplication/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "VolunteerID,Name,Address,City,Email,ContactNumber,Language,WhyInterested,PastVolunteer,OpportunityID")] VolunteerApplication volunteerApplication)
        {
            if (ModelState.IsValid)
            {
                db.Entry(volunteerApplication).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OpportunityID = new SelectList(db.VolunteerOpportunities, "OpportunityID", "OpportunityName", volunteerApplication.OpportunityID);
            return View(volunteerApplication);
        }


        // Only admin can delete volunteers 
        // GET: VolunteerApplication/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VolunteerApplication volunteerApplication = db.VolunteerApplications.Find(id);
            if (volunteerApplication == null)
            {
                return HttpNotFound();
            }
            return View(volunteerApplication);
        }

        // Only admin can delete volunteers 
        // POST: VolunteerApplication/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            VolunteerApplication volunteerApplication = db.VolunteerApplications.Find(id);
            db.VolunteerApplications.Remove(volunteerApplication);
            db.SaveChanges();
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

        public ActionResult Success()
        {
            return View();
        }
    }
}
