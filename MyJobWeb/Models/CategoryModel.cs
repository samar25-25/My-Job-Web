using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyJobWeb.Models
{
    public class CategoryModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Category Name is required!")]
        [StringLength(maximumLength: 200, MinimumLength = 5, ErrorMessage = "Category Name should be less than 200 and more than 5 letters")]
        public string Name { get; set; }



        public int ParentCategory { get; set; }


        public int ParentId { get; set; }
        public string ParentName { get; set; }

        public SelectList MainCategories { get; set; }
        public virtual ICollection<JobdetailsModel> Jobs { get; set; }
    }
}