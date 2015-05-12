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

        [HttpGet]
        public ActionResult Index()
        {
            SetUserFeedback();
            EventFeedViewModel feed;
            // Gets userId of the user viewing the site
            int userId = service.GetUserId(User.Identity.Name);
            // Gets the correct feed for the userId
            feed = service.GetEventFeed(userId);
            // Returns the feed to the view
            return View("Index", feed);
        }

        [HttpGet]
        public ActionResult AddFriend()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddFriend(string username)
        {
            User user = service.GetUser(User.Identity.Name);
            User friend = service.GetUser(username);
            User parameter = null;
            if (friend == null)
            {
                TempData["message"] = new Message("The username " + username + " could not be found.", MessageType.INFORMATION);
            }
            else if (User.Identity.Name == friend.UserName)
            {
                TempData["message"] = new Message("You can't add yourself to your friend list.", MessageType.INFORMATION);
            }
            else if (service.IsFriendsWith(user.UserID, friend.UserID))
            {
                TempData["message"] = new Message(username + " is already your friend.", MessageType.INFORMATION);
            }
            else if (service.SendFriendRequest(user.UserID, friend.UserID))
            {
                TempData["message"] = new Message(friend.UserName + " is now you friend.", MessageType.SUCCESS);
                parameter = friend;
            }
            else
            {
                TempData["message"] = new Message("Could not process Add Friend request please try again later.", MessageType.ERROR);
            }
            if (Request.IsAjaxRequest())
            {
                return Json(new { friend = parameter, message = TempData["message"] as Message }, JsonRequestBehavior.AllowGet);
            }
            return RedirectToAction("Index");
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
            User friend = service.GetUser(friendId);
            User parameter = null;

            if (service.IsFriendsWith(user.UserID, friendId))
            {
                TempData["message"] = new Message("You are no longer friends with " + friend.DisplayName, MessageType.SUCCESS);
                if (service.RemoveFriend(user.UserID, friendId))
                {
                    parameter = friend;
                }
            }
            else
            {
                TempData["message"] = new Message("You are not friends with " + friend.DisplayName, MessageType.ERROR);
            }
            if (Request.IsAjaxRequest())
            {
                return Json(new { friend = parameter, message = TempData["message"] as Message }, JsonRequestBehavior.AllowGet);
            }
            return RedirectToAction("Index");
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
                newEvent.Minutes = 1;
                newEvent.Active = true;
                newEvent.Min = 2;
                newEvent.Max = 4;
                service.CreateEvent(ref newEvent);
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
        public ActionResult Comment(int eventId, string content)
        {
            if (String.IsNullOrEmpty(content))
            {
                return RedirectToAction("Index");
            }
            Comment myComment = new Comment();
            myComment.Content = content;
            myComment.Active = true;
            myComment.OwnerId = service.GetUserId(User.Identity.Name);
            myComment.CreationTime = DateTime.Now;
            service.CreateComment(eventId, ref myComment);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult ChangeName()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangeName(User myUser)
        {
            User user = service.GetUser(User.Identity.Name);
            if (ModelState.IsValid)
            {
                service.ChangeDisplayName(user.UserID, myUser.DisplayName);
                TempData["message"] = new Message("Your name has been changed to " + myUser.DisplayName, MessageType.SUCCESS);
                return Index();
            }
            else
            {
                ModelState.AddModelError("Error", "Please enter a valid name.");
                return View();
            }
        }

        [HttpPost]
        public ActionResult AnswerEvent(int eventId, bool answer)
        {
            User user = service.GetUser(User.Identity.Name);
            if (service.IsInvitedToEvent(user.UserID, eventId))
            {
                if (service.AnswerEvent(user.UserID, eventId, answer))
                {
                    Event theEvent = service.GetEventById(eventId);
                    if (answer)
                    {
                        TempData["message"] = new Message("You are now an attendee of " + theEvent.Name, MessageType.SUCCESS);
                    }
                    else
                    {
                        TempData["message"] = new Message("You have successfully declined " + theEvent.Name, MessageType.SUCCESS);
                    }
                }
                else
                {
                    TempData["message"] = new Message("An error occured when processing your request, please try again later.", MessageType.ERROR);
                }
            }
            else
            {
                TempData["message"] = new Message("Either the event you are trying to access doesn't exist or you do not have sufficient access to it.", MessageType.INFORMATION);
            }
            return RedirectToAction("Index");
        }

        private void SetUserFeedback()
        {
            Message message = TempData["message"] as Message;
            if (message != null)
            {
                ViewBag.ErrorMessage = message.ErrorMessage;
                ViewBag.WarningMessage = message.WarningMessage;
                ViewBag.InformationMessage = message.InformationMessage;
                ViewBag.SuccessMessage = message.SuccessMessage;
            }
        }
    }
}