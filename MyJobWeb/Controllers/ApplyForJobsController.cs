using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyJobWeb.Data;
using System.IO;
using Microsoft.AspNet.Identity;

namespace MyJobWeb.Controllers
{
    public class ApplyForJobsController : Controller
    {
        private MyJobWebEntities db = new MyJobWebEntities();

        // GET: ApplyForJobs
        public ActionResult Index()
        {
            var applyForJobs = db.ApplyForJobs.Include(a => a.AspNetUser).Include(a => a.JobDetail);
            return View(applyForJobs.ToList());
        }

        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplyForJob applyForJob = db.ApplyForJobs.Find(id);
            if (applyForJob == null)
            {
                return HttpNotFound();
            }
            ViewBag.User_Id = new SelectList(db.AspNetUsers, "Id", "Email", applyForJob.User_Id);
            ViewBag.Job_Id = new SelectList(db.JobDetails, "Id", "Job_title", applyForJob.Job_Id);
            return View(applyForJob);
        }

        // POST: ApplyForJobs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Message,Job_Id,User_Id,ApplyDate,File_Id,File_Content")] ApplyForJob applyForJob, HttpPostedFileBase Upload)
        {

            if (ModelState.IsValid)
            {
                string path = Path.Combine(Server.MapPath("~/FileUploads"), Upload.FileName);
                Upload.SaveAs(path);
                applyForJob.File_Id = Upload.FileName;
                applyForJob.User_Id = User.Identity.GetUserId();
                applyForJob.ApplyDate = DateTime.Now;
                db.Entry(applyForJob).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index","Home");
            }

            ViewBag.Job_Id = new SelectList(db.JobDetails, "Id", "Job_title", applyForJob.Job_Id);
            return View(applyForJob);
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
