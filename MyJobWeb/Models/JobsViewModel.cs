using MyJobWeb.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyJobWeb.Models
{
    public class JobsViewModel
    {
        public IEnumerable<HttpPostedFileBase> files { get; set; }
        public string File { get; set; }
        public long Size { get; set; }
        public string Type { get; set; }



        public string job_title  { get; set; }
        
        public IEnumerable<ApplyForJob> items{ get; set; }
        
    }
}