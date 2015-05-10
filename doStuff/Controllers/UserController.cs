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
            EventFeedViewModel feed;
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
            User user = service.GetUser(User.Identity.Name);
            User friend = service.GetUser(newFriend.UserName);
            if (friend == null)
            {
                ModelState.AddModelError("Error", "The username " + newFriend.UserName + " could not be found.");
                return View();
            }
            if (User.Identity.Name == friend.UserName)
            {
                ModelState.AddModelError("Error", "You can't add yourself to your friend list.");
                return View();
            }
            if (service.IsFriendsWith(user.UserID, friend.UserID))
            {
                ModelState.AddModelError("Error", newFriend.UserName + " is already your friend.");
                return View();
            }
            if (service.SendFriendRequest(user.UserID, friend.UserID))
            {
                ViewBag.SuccessMessage = "Success, " + friend.UserName + " is now you friend.";
                ModelState.Clear();
                return View();
            }
            ViewBag.ErrorMessage = "An error occured when processing your request, please try again later.";
            return View("Index", service.GetEventFeed(user.UserID));
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
            return RedirectToAction("ViewFriendRequests");
        }

        [HttpPost]
        public ActionResult RemoveFriend(int friendId)
        {
            User user = service.GetUser(User.Identity.Name);

            if (service.IsFriendsWith(user.UserID, friendId))
            {
                User friend = service.GetUser(friendId);
                ViewBag.SuccessMessage = "You are no longer friends with " + friend.DisplayName;
                service.RemoveFriend(user.UserID, friendId);
                return RedirectToAction("Index");
            }

            return RedirectToAction("Error", "An error occured when handeling your request, please try again later.");
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
        [HttpGet]
        public ActionResult Comment()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Comment(int eventId, Comment myComment)
        {
            service.CreateComment(eventId, myComment);
            return View();
        }

        [HttpGet]
        public ActionResult ChangeName()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangeName(User myUser)
        {
            int myId = service.GetUserId(User.Identity.Name);
            service.ChangeDisplayName(myId, myUser.DisplayName);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AnswerEvent(int eventId, bool answer)
        {
            User user = service.GetUser(User.Identity.Name);

            if(service.IsInvitedToEvent(user.UserID, eventId))
            {
                if(service.AnswerEvent(user.UserID, eventId, answer))
                {
                    return RedirectToAction("Index");
                }
                ViewBag.ErrorMessage = "An error occured when processing your request, please try again later.";
                return RedirectToAction("Index");
            }
            ViewBag.ErrorMessage = "Either the event you are trying to access doesn't exist or you do not have sufficient access to it.";
            return RedirectToAction("Index");
        }
    }
}