using AutoMapper;
using Microsoft.AspNet.Identity;
using MyJobWeb.App_Start;
using MyJobWeb.Data;
using MyJobWeb.Models;
using MyJobWeb.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace MyJobWeb.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();
        private MyJobWebEntities db = new MyJobWebEntities();


        private readonly JobdtailsServices jobsService;
        private readonly IMapper mapper;

        public HomeController()
        {
            jobsService = new JobdtailsServices();
            mapper = AutoMapperConfig.Mapper;
        }


        

        public ActionResult Index(string query = null, int? categoryId = null, string userId = null)
        {

            var courses = jobsService.ReadAll(query, userId, categoryId);


            return View(mapper.Map<List<JobDetail>, List<JobdetailsModel>>(courses));


        }


        public ActionResult About()
        {
            ViewBag.Message = "This website is made to help you find a perfect"+
               " job suitable to you and for the Puplisher"+
                " who is looking for woorkers for his Department";

            return View();
        }
        

        public ActionResult Contact()
        {
           

            return View();
        }
        [HttpPost]
        public ActionResult Contact(ContactModel contact)
        {
            var mail = new MailMessage();
            var loginInfo = new NetworkCredential("sooka2225@gmail.com", "zxc123@Z");
            mail.From = new MailAddress(contact.Email);
            mail.To.Add(new MailAddress("sooka2225@gmail.com"));
            mail.Subject = contact.Subject;
            mail.IsBodyHtml = true;
            string body = "اسم المرسل" + contact.Name + "<br>" + "بريد المرسل:" + contact.Email + "<br>" +
                "عنوان المرسل:" +
                contact.Subject + "<br>" + "نص الرساله" + contact.Message;
            mail.Body = body;
            using (SmtpClient client = new SmtpClient())
            {
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("sooka2225@gmail.com", "zxc123@Z");
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                client.Send(mail);
            }

            return RedirectToAction("Index");
        }
        [Authorize]
        public ActionResult Apply()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Apply(string Message, HttpPostedFileBase Upload)
        {

            var userId = User.Identity.GetUserId();
            var jobId = (int)Session["Job_Id"];
            var check = db.ApplyForJobs.Where(a => a.User_Id == userId && a.Job_Id == jobId).ToList();
            if (check.Count < 1)
            {
                var job = new ApplyForJob();
                string path = Path.Combine(Server.MapPath("~/FileUploads"), Upload.FileName);
                Upload.SaveAs(path);
                job.File_Id = Upload.FileName;
                job.User_Id = userId;
                job.Message = Message;
                job.Job_Id = jobId;
                job.ApplyDate = DateTime.Now;
                db.ApplyForJobs.Add(job);
                db.SaveChanges();
                ViewBag.Result = "تم اختيار الوظيفه";

            }
            else
            {
                ViewBag.Result = "لقد تقدمت لهذه الوظيفه بالفعل ";
            }
            return View();
        }



       
        public ActionResult GetJobByUser()
        {
            var userId = User.Identity.GetUserId();
            var jobs = db.ApplyForJobs.Where(a => a.User_Id == userId);
            
            return View(jobs.ToList());
        }
        public ActionResult DetailsOfJob(int id)
        {
            var job = db.ApplyForJobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            
            }
            return View(job);
        }
        public ActionResult Details(int id)
        {
            var job = db.JobDetails.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            Session["Job_Id"] = id;
            return View(job);
        }
        //Edit apply for job
        public ActionResult Edit(int Id)
        {
            var job = db.ApplyForJobs.Find(Id);
            if (job == null)
            {
                return HttpNotFound();
            }

            return View(job);
        }
        public ActionResult Edit(ApplyForJob job)
        {
            if (ModelState.IsValid)
            {
                db.Entry(job).State = EntityState.Modified;
                job.ApplyDate = DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("GetJobByUser");
            }
            return View(job);
        }
        

        [Authorize(Roles = "Puplisher")]
        public ActionResult GetJobsByBublisher()
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();




                var jobs = from app in db.ApplyForJobs
                           join job in db.JobDetails
                           on app.Job_Id equals job.Id
                           where job.User_Id == userId
                           select app;
                var grouped = from j in jobs
                              group j by j.JobDetail.Job_title
                            into gr
                              select new JobsViewModel
                              {
                                  job_title = gr.Key,
                                  items = gr
                              };


                ViewBag.User_Id = new SelectList(db.AspNetUsers, "Id", "Email");
                return View(grouped.ToList());
            }
            return View(db.ApplyForJobs.ToList());

        }
        public ActionResult Search()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Search(string searchName)
        {
            var result = db.JobDetails.Where(a => a.Job_title.ToLower().Contains(searchName)
             || a.Job_desc.ToLower().Contains(searchName) ||
             a.Category.Name.ToLower().Contains(searchName)
            );

            return View(result);
        }
        //public FileResult Download()
        //{

        //    string path = Server.MapPath("~/FileUploads");
        //    string fileName = Path.GetFileName("Samar Essam cv.docx");
        //    string fullpath = Path.Combine(path, fileName);
        //    return File(fullpath, "file/docx", "Samar Essam cv.docx");
        //}
        //public FileResult Download()
        //{
        //    string fullPath = Path.Combine(Server.MapPath("~/FileUploads"));
        //    byte[] fileBytes = System.IO.File.ReadAllBytes(fullPath);



        //    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet);
        //}

        //private string GetFileTypeByExtension(string fileExtension)
        //{
        //    switch (fileExtension.ToLower())
        //    {
        //        case ".docx":
        //        case ".doc":
        //            return "Microsoft Word Document";
        //        case ".xlsx":
        //        case ".xls":
        //            return "Microsoft Excel Document";
        //        case ".txt":
        //            return "Text Document";
        //        case ".jpg":
        //        case ".png":
        //            return "Image";
        //        default:
        //            return "Unknown";
        //    }
        //}
        //public ActionResult FileUpload()
        //{
        //    var items = GetFiles();
        //    return View(items);

        //}
        //[HttpPost]
        //public ActionResult FileUpload(HttpPostedFileBase file)
        //{
        //    if (file != null && file.ContentLength > 0)
        //    {
        //        try
        //        {
        //            string path = Path.Combine(Server.MapPath("~/FileUploads"), Path.GetFileName(file.FileName));
        //            file.SaveAs(path);
        //            ViewBag.Message = "File Uploaded Succseefully";
        //        }
        //        catch (Exception ex)
        //        {
        //            ViewBag.Message = "Error:" + ex.Message.ToString();
        //        }
        //    }
        //    else
        //    {
        //        ViewBag.Message = " You Have not Specify A file";
        //    }

        //    return View();

      //  }
        //public FileResult Download(string FileName)
        //{
        //    var FileVirtualPath = "~/FileUploads" + FileName;
        //    return File(FileVirtualPath, "application/force- download", Path.GetFileName(FileVirtualPath));

        //}
        //private List<string> GetFiles()
        //{
        //    var dir = new DirectoryInfo(Server.MapPath("~/FileUploads"));
        //    FileInfo[] fileNames = dir.GetFiles("*.*");
        //    List<string> items=new List<string>();
        //    foreach(var file in fileNames)
        //    {
        //        items.Add(file.Name);
        //    }
        //    return items;

        //}
        public FileResult Download(string path)
        {

            string fileName = Path.GetFileName(path);
      
            
                string fullPath = Path.Combine(Server.MapPath("~/FileUploads"), fileName);
                byte[] fileBytes = System.IO.File.ReadAllBytes(fullPath);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            


           
            
        }

        public ActionResult Edit1(int? id)
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
        public ActionResult Edit1([Bind(Include = "Id,Message,Job_Id,User_Id,ApplyDate,File_Id,File_Content")] ApplyForJob applyForJob, HttpPostedFileBase Upload)
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
                return RedirectToAction("Index");
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