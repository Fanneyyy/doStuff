using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using doStuff.ViewModels;
using doStuff.Services;
using doStuff.Models;
using doStuff.Models.DatabaseModels;
using doStuff.Databases;
using ErrorHandler;
namespace doStuff.Controllers
{
    [Authorize]
    public class UserController : ParentController
    {
        private static Database db = new Database(null);

        [HttpGet]
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
            return View();
        }

        [HttpPost]
        public ActionResult AddFriend(User newFriend)
        {
                try
                {
                   int friendId = service.GetUserId(newFriend.UserName);
                   int userId = service.GetUserId(User.Identity.Name);
                   service.SendFriendRequest(userId, friendId);
                   service.AnswerFriendRequest(friendId, userId, true);
                   return RedirectToAction("Index");
                }
                catch(UserNotFoundException)
                {
                    ModelState.AddModelError("Error", "Username not found");
                    return View();
                }
        }
        [HttpGet]
        public ActionResult Banner()
        {
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult ViewFriendRequests()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AnswerFriendRequests(int userId, bool answer)
        {
            //TODO
            int myId = service.GetUserId(User.Identity.Name);
            service.AnswerFriendRequest(userId, myId, answer); 
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult RemoveFriend(int friendId)
        {
            //TODO
            int userId = service.GetUserId(User.Identity.Name);
            service.RemoveFriend(userId, friendId);
            return View();
        }

        [HttpGet]
        public ActionResult CreateEvent()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateEvent(Event newEvent)
        {
            if (ModelState.IsValid)
            {
                newEvent.CreationTime = DateTime.Now;
                newEvent.OwnerId = service.GetUserId(User.Identity.Name);
                newEvent.Minutes = 23;
                newEvent.Active = true;
                service.CreateEvent(newEvent);
                return RedirectToAction("Index");
            }

            return View(newEvent);
        }

        [HttpPost]
        public ActionResult RemoveEvent(int eventId)
        {
            //TODO
            if (service.RemoveEvent(eventId))
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Comment(int eventId, Comment myComment)
        {
            service.CreateComment(eventId, myComment);
            return View();
        }

        [HttpGet]
        public ActionResult ChangeUserName()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangeUserName(User myUser)
        {
            int myId = service.GetUserId(User.Identity.Name);
            service.ChangeDisplayName(myId, myUser.DisplayName);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AnswerEvent(FormCollection collection)
        {
            //apologies for very spooky code, but it should work
            //feel free to change
            int eventId;
            bool answer = "Yes" == collection.AllKeys[0];
            if (answer)
            {
                string id = collection.GetValue("Yes").AttemptedValue;
                eventId = Int32.Parse(id);
            }
            else
            {
                string id = collection.GetValue("No").AttemptedValue;
                eventId = Int32.Parse(id);
            }

            int myId = service.GetUserId(User.Identity.Name);
            service.AnswerEvent(myId, eventId, answer);
            return RedirectToAction("Index");
        }
    }
}