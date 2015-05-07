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
    //[Authorize]
    public class HomeController : ParentController
    {
        static Service service = new Service();
        [HttpGet]
        [Authorize]
        public ActionResult Index()
        {
            EventFeedViewModel feed = new EventFeedViewModel();
            // Gets userId of the user viewing the site
            int userId = service.GetUserId(User.Identity.Name);
            // Gets the correct feed for the userId
            feed = service.GetEventFeed(userId);
            // Returns the feed to the view
            return View(feed);
        }
    }
}