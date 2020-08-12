using MyJobWeb.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyJobWeb.Services
{
    public interface IJobdtailsServices
    {

        int Create(JobDetail jbobdetail);
        int UpdateJob(JobDetail updatedjob);
        List<JobDetail> ReadAll(string query, string puplisherId = null, int? categoryId = null);
        JobDetail ReadById(int Id);
        bool delete(int Id);
    }
    public class JobdtailsServices:IJobdtailsServices
    {
        private readonly MyJobWebEntities db;
        public JobdtailsServices()
        {
            db = new MyJobWebEntities();
        }
        public int Create(JobDetail jbobdetail)
        {

            AspNetUser user = new AspNetUser();
            jbobdetail.Creation_Date = DateTime.Now;
            jbobdetail.User_Id = user.Email;
            db.JobDetails.Add(jbobdetail);
            return db.SaveChanges();
        }

        public bool delete(int Id)
        {
            var job = ReadById(Id);
            if (job != null)
            {
                db.JobDetails.Remove(job);
                return db.SaveChanges() > 0 ? true : false;
            }
            return false;
        }

        public List<JobDetail> ReadAll(string query, string puplisherId = null, int? categoryId = null)
        {

            return db.JobDetails.Where(c => (puplisherId == null || c.User_Id == puplisherId)
                && (categoryId == null || c.Cat_Id == categoryId)
                && (query == null || c.Job_title.Contains(query))
              ).ToList();
        }

        public JobDetail ReadById(int Id)
        {
            return db.JobDetails.Find(Id);
        }

        public int UpdateJob(JobDetail updatedjob)
        {
            var jobName = updatedjob.Job_title.ToLower();
            var jobNameExcists = db.JobDetails.Where(c => c.Job_title.ToLower() != jobName);
            if (jobNameExcists.Where(c => c.Job_title.ToLower() == jobName).Any())
            {
                return -1;
            }
            db.JobDetails.Attach(updatedjob);
            db.Entry(updatedjob).State = System.Data.Entity.EntityState.Modified;
            return db.SaveChanges();
        }
    }
}