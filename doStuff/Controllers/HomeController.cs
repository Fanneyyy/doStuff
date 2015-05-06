using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace doStuff.Controllers
{
    [Authorize]
    public class HomeController : ParentController
    {
        public ActionResult Index()
        {
            //TODO
            return View();
        }
    }
}