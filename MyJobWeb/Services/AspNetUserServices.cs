using MyJobWeb.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyJobWeb.Services
{
    //public interface IAspNetUserServices
    //{
    //    int Create(AspNetUser User);
    //    AspNetUser FindByEmail(string Email);
    //    AspNetUser ReadById(int Id);
    //    IEnumerable<AspNetUser> ReadAll();
    //}
    //public class AspNetUserServices: IAspNetUserServices
    //{

    //    private readonly MyJobWebEntities db;
    //    public AspNetUserServices()
    //    {
    //        db = new MyJobWebEntities();
    //    }
    //    public int Create(AspNetUser User)
    //    {
    //        var PuplisherExcists = FindByEmail(User.Email);
    //        if (PuplisherExcists != null)
    //        {
    //            return -2;
    //        }

    //        db.AspNetUsers.Add(User);
    //        return db.SaveChanges();
    //    }

    //    public AspNetUser FindByEmail(string Email)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public IEnumerable<AspNetUser> ReadAll()
    //    {
    //        return db.AspNetUsers.Where(p => p.Email == Email).FirstOrDefault();

    //    }

    //    public AspNetUser ReadById(int Id)
    //    {
    //        return db.AspNetUsers;
    //    }
    //}
}
