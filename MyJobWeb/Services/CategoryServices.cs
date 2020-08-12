using MyJobWeb.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyJobWeb.Services
{


    public interface ICategoryService
    {
        int Update(Category updatedcategory);
        Category ReadById(int id);
        List<Category> ReadAll();
        int Create(Category newcategory);
        bool Delete(int id);
    }
    public class CategoryServices : ICategoryService
    {

        private readonly MyJobWebEntities db;
        public CategoryServices()
        {
            db = new MyJobWebEntities();

        }

        public int Create(Category newcategory)
        {
            var CategoryName = newcategory.Name.ToLower();
            var categorynameexcist = db.Categories.Where(c => c.Name.ToLower() == CategoryName).Any();
            if (categorynameexcist)
            {
                return -1;

            }
            db.Categories.Add(newcategory);

            return db.SaveChanges();
        }

        public bool Delete(int id)
        {
            var category = ReadById(id);
            if (category != null)
            {
                db.Categories.Remove(category);

                return db.SaveChanges() > 0 ? true : false;

            }
            return false;
        }

        public List<Category> ReadAll()
        {
            return db.Categories.ToList();
        }

        //public List<Category> ReadAll()
        //{
        //   return db.Categories.ToList();
        //}

        public Category ReadById(int id)
        {
            return db.Categories.Find(id);
        }

        public int Update(Category updatedcategory)
        {
            var CategoryName = updatedcategory.Name.ToLower();
            var categorynameexcist = db.Categories.Where(c => c.Name.ToLower() != CategoryName);
            if (categorynameexcist.Where(c => c.Name.ToLower() == CategoryName).Any())
            {
                return -2;

            }

            db.Categories.Attach(updatedcategory);
            db.Entry(updatedcategory).State = System.Data.Entity.EntityState.Modified;
            return db.SaveChanges();
        }







    }
   
}