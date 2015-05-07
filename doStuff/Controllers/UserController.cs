using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using doStuff.ViewModels;
using doStuff.Services;
using doStuff.Models;
using doStuff.Models.DatabaseModels;

namespace doStuff.Controllers
{
    public class UserController : ParentController
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

        [HttpGet]
        public ActionResult AddFriend()
        {
            //TODO

            return View();
        }

        [HttpPost]
        public ActionResult AddFriend(FormCollection collection)
        {
            //TODO
            return View();
        }

        [HttpGet]
        public ActionResult ViewFriendRequests()
        {
            //TODO
            return View();
        }

        [HttpPost]
        public ActionResult AnswerFriendRequests(uint userId, bool answer)
        {
            //TODO
            return View();
        }

        [HttpPost]
        public ActionResult RemoveFriend(uint friendId)
        {
            //TODO
            return View();
        }

        [HttpGet]
        public ActionResult CreateEvent()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateEvent(/*[Bind(Include = "Id,Title,Text,DateCreated,Category")], */Event newEvent)
        {
            if (ModelState.IsValid)
            {
                service.CreateEvent(newEvent);
                return RedirectToAction("Index");
            }

            return View(newEvent);
        }

        [HttpPost]
        public ActionResult RemoveEvent(int eventId)
        {
            //TODO
            return View();
        }

        [HttpPost]
        public ActionResult Comment(int eventId, FormCollection collection)
        {
            //TODO
            return View();
        }

        [HttpGet]
        public ActionResult ChangeUserName()
        {
            //TODO

            return View();
        }

        [HttpPost]
        public ActionResult ChangeUserName(Event eventToChange)
        {
            //TODO

            return View();
        }

        [HttpPost]
        public ActionResult AnswerEvent(int eventId, bool answer)
        {
            //TODO
            return View();
        }
    }
}