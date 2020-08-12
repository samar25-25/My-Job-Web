using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyJobWeb.Models
{
    public class JobdetailsModel
    {
      
        public int Job_Id { get; set; }
        [DisplayName("Job Title")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Job title is required")]
        public string Job_title { get; set; }
        [DisplayName("Job Describtion")]
        [AllowHtml]
        public string Job_desc { get; set; }
        [DisplayName("Upload Image")]
        public string Job_Images { get; set; }


        public HttpPostedFileBase ImageFile { get; set; }
        [DisplayName(" Category Name")]
        public int Category_Id { get; set; }

        public string Category_Name { get; set; }

        public SelectList Categories { get; set; }
        // public int Puplisher_Id { get; set; }

        public string UserId { get; set; }
        private string _imageId;
        public string Image_ID
        {
            set
            {
                _imageId = string.IsNullOrWhiteSpace(value) ? "FB_IMG_1475223780880.jpg" : value;

            }
            get
            {
                return _imageId;
            }
        }
        public string PuplisherName { get; set; }
        public SelectList Puplishers { get; set; }
        public SelectList Users { get; set; }
    }
    public class jobsListModel
    {
        public IEnumerable<JobdetailsModel> jobs { get; set; }
        public string Query { get; set; }
        public string PuplisherId { get; set; }
        public SelectList Puplishers { get; set; }
        public SelectList Categories { get; set; }
        public int CategoryId { get; set; }
    }
}