using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using doStuff.Services;
using doStuff.Databases;
using doStuff.Models.DatabaseModels;
using doStuff.ViewModels;

namespace doStuff.Controllers
{
    [Authorize]
    public class HomeController : ParentController
    {

        public ActionResult Index()
        {
            return RedirectToAction("Index", "User");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}