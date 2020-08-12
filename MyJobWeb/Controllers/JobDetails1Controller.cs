using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyJobWeb.Data;
using Microsoft.AspNet.Identity;
using System.IO;

namespace MyJobWeb.Controllers
{
   
    public class JobDetails1Controller : Controller
    {
        private MyJobWebEntities db = new MyJobWebEntities();



       
       
        public ActionResult IndexSearch(string searching)
        {
            return View(db.JobDetails.Where(x => x.Job_title.Contains(searching)||x.Category.Name.Contains(searching) || searching == null).ToList());
        }
        // GET: JobDetails1
        [Authorize(Roles = "Puplisher,Admin")]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var jobDetails = db.JobDetails.Where(x=>x.AspNetUser.Id==userId).Include(j => j.AspNetUser).Include(j => j.Category);
            return View(jobDetails.ToList());
        }

        // GET: JobDetails1/Details/5
        [Authorize(Roles = "Puplisher,Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobDetail jobDetail = db.JobDetails.Find(id);
            if (jobDetail == null)
            {
                return HttpNotFound();
            }
            return View(jobDetail);
        }
        [Authorize(Roles = "Puplisher,Admin")]
        // GET: JobDetails1/Create
        public ActionResult Create()
        {
           // ViewBag.User_Id = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.Cat_Id = new SelectList(db.Categories, "Id", "Name");
            return View();
        }

        // POST: JobDetails1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Job_title,Job_desc,Image_Id,User_Id,Cat_Id,Creation_Date")] JobDetail jobDetail, HttpPostedFileBase Upload)
        {
            if (ModelState.IsValid)
            {
                string path = Path.Combine(Server.MapPath("~/Uploads"), Upload.FileName);
                Upload.SaveAs(path);
                jobDetail.Image_Id = Upload.FileName;
                jobDetail.User_Id = User.Identity.GetUserId();
                jobDetail.Creation_Date = DateTime.Now;
                db.JobDetails.Add(jobDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

           // ViewBag.User_Id = new SelectList(db.AspNetUsers, "Id", "Email", jobDetail.User_Id);
            ViewBag.Cat_Id = new SelectList(db.Categories, "Id", "Name", jobDetail.Cat_Id);
            return View(jobDetail);
        }
        [Authorize(Roles = "Puplisher,Admin")]

        // GET: JobDetails1/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobDetail jobDetail = db.JobDetails.Find(id);
            if (jobDetail == null)
            {
                return HttpNotFound();
            }
            
            ViewBag.Cat_Id = new SelectList(db.Categories, "Id", "Name", jobDetail.Cat_Id);
            return View(jobDetail);
        }

        // POST: JobDetails1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Job_title,Job_desc,Image_Id,User_Id,Cat_Id,Creation_Date")] JobDetail jobDetail, HttpPostedFileBase Upload)
        {
            if (ModelState.IsValid)
            {
                
             
                    string path = Path.Combine(Server.MapPath("~/Uploads"), Upload.FileName);
                    Upload.SaveAs(path);
                    jobDetail.Image_Id = Upload.FileName;
                    jobDetail.User_Id = User.Identity.GetUserId();
                    jobDetail.Creation_Date = DateTime.Now;
                    db.Entry(jobDetail).State = EntityState.Modified;
                    db.SaveChanges();
               
                
               
               
                return RedirectToAction("Index");
            }
       
            ViewBag.Cat_Id = new SelectList(db.Categories, "Id", "Name", jobDetail.Cat_Id);
            return View(jobDetail);
        }
        [Authorize(Roles = "Puplisher,Admin")]

        // GET: JobDetails1/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobDetail jobDetail = db.JobDetails.Find(id);
            if (jobDetail == null)
            {
                return HttpNotFound();
            }
            return View(jobDetail);
        }

        // POST: JobDetails1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            JobDetail jobDetail = db.JobDetails.Find(id);
            db.JobDetails.Remove(jobDetail);
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
    }
}
