using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using doStuff.Services;
using doStuff.Databases;
using doStuff.Models.DatabaseModels;

namespace doStuff.Controllers
{
    //[Authorize]
    public class HomeController : ParentController
    {
        private static UserService service = new UserService();
        private static DatabaseContext db = new DatabaseContext();

        public ActionResult Index()
        { 
            /*UserTable user = new UserTable();
            user.Email = "Ironpeak";
            db.Users.Add(user);
            db.SaveChanges();
            var ret = (from p in db.Users select p).First(); */
            return View();
        }
    }
}