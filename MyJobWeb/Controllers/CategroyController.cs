using System;
using AutoMapper;
using MyJobWeb.App_Start;
using MyJobWeb.Data;
using MyJobWeb.Models;
using MyJobWeb.Services;

using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyJobWeb.Controllers
{
    [Authorize(Roles ="Puplisher")]
    public class CategroyController : Controller
    {
        private readonly CategoryServices categoryservice;
        private readonly IMapper mapper;
        private MyJobWebEntities db;
        public CategroyController()
        {
            categoryservice = new CategoryServices();
            mapper = AutoMapperConfig.Mapper;
            db = new MyJobWebEntities();
        }

        // GET: Admin/Category
        public ActionResult Index()
        {
            var categories = categoryservice.ReadAll();
            var categorieslist = mapper.Map<List<CategoryModel>>(categories);
            return View(categorieslist);
        }
        //Get
        public ActionResult Create()
        {
            var categoryModel = new CategoryModel();
            initMainCategory(null, ref categoryModel);
            //    var Categorieslist = categoryservice.ReadAll();
            //    var CategoryModel = new CategoryModel
            //    {
            //        MainCategories = new SelectList(Categorieslist, "ID", "Name")
            //};
            return View(categoryModel);
        }
        //post
        [HttpPost]
        public ActionResult Create(CategoryModel data)
        {
            var newCategory = mapper.Map<Category>(data);
            newCategory.Category1 = null;

            //int creationresult= categoryservice.Create(new Data.Category { Name = data.Name,Parent_Id=data.ParentId});
            int creationresult = categoryservice.Create(newCategory);
            if (creationresult == -1)
            {
                initMainCategory(null, ref data);

                ViewBag.Message = "Category Name already excist";
                return View(data);
            }
            return RedirectToAction("Index");

        }

        public ActionResult Update(int? id)
        {
            if (id == null || id == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            var currentCategory = categoryservice.ReadById(id.Value);
            if (currentCategory == null)
            {
                return HttpNotFound($"this category ({id }) not found");
            }
            var CategoryModel = new CategoryModel
            {
                Id = currentCategory.Id,
                Name = currentCategory.Name,
                ParentId = currentCategory.Parent_Id.Value



            };
            initMainCategory(currentCategory.Id, ref CategoryModel);
            return View(CategoryModel);


        }
        [HttpPost]
        public ActionResult Update(CategoryModel data)
        {
            if (ModelState.IsValid)
            {
                var updatedCategory = new Category
                {
                    Id = data.Id,
                    Name = data.Name,
                    Parent_Id = data.ParentId


                };
                var result = categoryservice.Update(updatedCategory);

                if (result == -1)
                {
                    ViewBag.Message = "Category Name already excist";
                    initMainCategory(data.Id, ref data);
                    return View(data);
                }


                else if (result > 0)
                {
                    ViewBag.Success = true;
                    ViewBag.Message = $"Category ({data.Id}) Updated successfuly";


                }
                else
                {
                    ViewBag.Message = $"An error occured while updating!!";
                }
            }

            return View(data);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return RedirectToAction("index", "Home");
            }
            var currentcategory = categoryservice.ReadById(id.Value);
            if (currentcategory == null)
            {
                return HttpNotFound($"this category ({id }) not found");
            }
            var CategoryModel = new CategoryModel
            {
                Id = currentcategory.Id,
                Name = currentcategory.Name,
                ParentId = currentcategory.Parent_Id.Value



            };
            initMainCategory(currentcategory.Id, ref CategoryModel);
            return View(CategoryModel);
        }



        [HttpPost]
        public ActionResult Edit(CategoryModel data)
        {

            var updatedCategory = new Category
            {
                Id= data.Id,
                Name = data.Name,
                Parent_Id = data.ParentId
            };
            var result = categoryservice.Update(updatedCategory);

            if (result == -1)
            {
                ViewBag.Message = "Category Name already excist";
                initMainCategory(data.Id, ref data);
                return View(data);
            }


            else if (result > 0)
            {
                ViewBag.Success = true;
                ViewBag.Message = $"Category ({data.Id}) Updated successfuly";


            }
            else
                ViewBag.Message = $"An error occured while updating!!";

            return View(data);
        }

        private void initMainCategory(int? excludeCurrent, ref CategoryModel CategoryModel)
        {

            var categoryList = categoryservice.ReadAll();

            if (excludeCurrent != null)
            {
                var currentCategory = categoryList.Where(c => c.Id == excludeCurrent).FirstOrDefault();
                categoryList.Remove(currentCategory);
            }

            CategoryModel.MainCategories = new SelectList(categoryList, "Id", "Name");

        }



        //private void IninSelectlist(int? excludeCurrent, ref CategoryModel jobdetailsmodel)
        //{
        //    var categories = categoryservice.ReadAll();
        //    var mappedcategorylist = mapper.Map<IEnumerable<CategoryModel>>(categories);
        //    jobdetailsmodel.MainCategories = new SelectList(mappedcategorylist, "Cat_Id", "Cat_Name");


        //    var users = userservices.ReadAll();
        //    var mappeduserlist = mapper.Map<IEnumerable<UserModel>>(users);
        //    jobdetailsmodel.Users = new SelectList(mappeduserlist, "User_Id", "User_First_Name");


        //    var jobList = jobservice.ReadAll();
        //    if (excludeCurrent != null)
        //    {
        //        var currentCategory = jobList.Where(c => c.Cat_Id == excludeCurrent).FirstOrDefault();
        //        jobList.Remove(currentCategory);
        //    }

        //    jobdetailsmodel.Categories = new SelectList(jobList, "Cat_Id", "Cat_Name");




        //}
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        public ActionResult Delete(int? Id)
        {
            if (Id != null)
            {
                var category = categoryservice.ReadById(Id.Value);
                var categoryInfo = new CategoryModel
                {
                    Id = category.Id,
                    Name = category.Name,
                    ParentName = category.Category1?.Name
                };
                return View(categoryInfo);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteConfirmed(int? Id)
        {

            if (Id != null)
            {
                var deleted = categoryservice.Delete(Id.Value);
                if (deleted) { return RedirectToAction("Index"); }

                return RedirectToAction("Delete", new
                {
                    Id
                    = Id
                });

            }

            return HttpNotFound();

        }
    }
}