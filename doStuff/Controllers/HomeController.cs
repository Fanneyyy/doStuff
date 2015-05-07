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
        /*private DatabaseContext db = new DatabaseContext();
        public ActionResult Index()
        {
            var user = (from u in db.Users
                        select u).ToList();

            foreach(var u in user)
            {
                throw new Exception();
            }

            db.SaveChanges();
            return View("Error");
        }*/
    }
}