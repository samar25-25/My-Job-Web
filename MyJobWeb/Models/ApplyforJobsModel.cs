using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyJobWeb.Models
{
    public class ApplyforJobsModel
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime ApplyDate { get; set; }

        public int JobId { get; set; }


        public string UserId { get; set; }
        public string FileId { get; set; }

        public virtual JobdetailsModel job { get; set; }
        public virtual ApplicationUser user { get; set; }
    }
}