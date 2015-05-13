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
using CustomErrors;

namespace doStuff.Controllers
{
    [Authorize]
    public class UserController : ParentController
    {

        [HttpGet]
        [HandleError]
        public ActionResult Index()
        {
            SetUserFeedback();
            EventFeedViewModel feed;

            User user = service.GetUser(User.Identity.Name);

            feed = service.GetEventFeed(user.UserID);

            if (Request.IsAjaxRequest())
            {
                return Json(RenderPartialViewToString("EventFeed", feed), JsonRequestBehavior.AllowGet);
            }

            return View("Index", feed);
        }

        [HttpPost]
        public ActionResult AddFriend(string username)
        {
            User user = service.GetUser(User.Identity.Name);
            User friend = service.GetUser(username);
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
            else if (service.FriendRequestExists(user.UserID, friend.UserID))
            {
                TempData["message"] = new Message(username + " still has a pending friend request", MessageType.INFORMATION);
            }
            else if (service.SendFriendRequest(user.UserID, friend.UserID))
            {
                TempData["message"] = new Message(username + " has received your friend request.", MessageType.SUCCESS);
            }
            else
            {
                TempData["message"] = new Message("Could not process Add Friend request please try again later.", MessageType.ERROR);
            }
            if (Request.IsAjaxRequest())
            {
                return Json(new { message = TempData["message"] as Message }, JsonRequestBehavior.AllowGet);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AcceptFriendRequest(int requesterId)
        {
            User user = service.GetUser(User.Identity.Name);
            User friend = service.GetUser(requesterId);
            if(service.IsFriendsWith(user.UserID, requesterId))
            {
                TempData["message"] = new Message(friend.DisplayName + " is already your friend", MessageType.INFORMATION);
            }
            else if(service.AnswerFriendRequest(friend.UserID, user.UserID, true))
            {
                TempData["message"] = new Message("You are now friends with " + friend.DisplayName, MessageType.SUCCESS);
            }
            else
            {
                TempData["message"] = new Message("The friend request you are trying to access no longer exists.", MessageType.SUCCESS);
            }
            if (Request.IsAjaxRequest())
            {
                return Json(new { message = TempData["message"] as Message }, JsonRequestBehavior.AllowGet);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeclineFriendRequest(int requesterId)
        {
            User user = service.GetUser(User.Identity.Name);
            User friend = service.GetUser(requesterId);
            if(service.AnswerFriendRequest(requesterId, user.UserID, false))
            {
                TempData["message"] = new Message("You declined a friend request from " + friend.DisplayName, MessageType.SUCCESS);
            }
            else
            {
                TempData["message"] = new Message("Could not find that specific friend request, maybe the other user withdrew it.", MessageType.ERROR);
            }
            if (Request.IsAjaxRequest())
            {
                return Json(new { friend = friend, message = TempData["message"] as Message }, JsonRequestBehavior.AllowGet);
            }
            return RedirectToAction("Index");
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
            else if(service.FriendRequestExists(user.UserID, friendId))
            {
                if (service.FriendRequestCancel(user.UserID, friendId))
                {
                    parameter = friend;
                }
                TempData["message"] = new Message("Friend request to " + friend.DisplayName + " has been cancelled", MessageType.SUCCESS);
            }
            else
            {
                if (friend != null)
                {
                    TempData["message"] = new Message("You are not friends with " + friend.DisplayName, MessageType.ERROR);
                }
                else
                {
                    TempData["message"] = new Message("User could not be found", MessageType.ERROR);
                }
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
                newEvent.Active = true;
                newEvent.Max = 100;
                if (service.CreateEvent(ref newEvent))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("Error", "An error occured when creating your event, please try again later.");
                }
            }
            return View(newEvent);
        }

        [HttpPost]
        public ActionResult Comment(int eventId, string content)
        {
            if (String.IsNullOrEmpty(content))
            {
                return RedirectToAction("Index");
            }
            User user = service.GetUser(User.Identity.Name);
            if(service.IsInvitedToEvent(user.UserID, eventId))
            {
                Comment myComment = new Comment();
                myComment.Content = content;
                myComment.Active = true;
                myComment.OwnerId = service.GetUserId(User.Identity.Name);
                myComment.CreationTime = DateTime.Now;
                if (service.CreateComment(eventId, ref myComment))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Message"] = new Message("An Error occured when processing your event, please try again later", MessageType.ERROR);
                }
            }
            else
            {
                TempData["message"] = new Message("Either the event you are trying to access doesn't exist or you do not have sufficient access to it.", MessageType.INFORMATION);
            }
            if (Request.IsAjaxRequest())
            {
                return Json(new { message = TempData["message"] as Message }, JsonRequestBehavior.AllowGet);
            }
            return RedirectToAction("Index");
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
                        TempData["message"] = new Message("You are listed as an attendee of " + theEvent.Name, MessageType.SUCCESS);
                    }
                    else
                    {
                        TempData["message"] = new Message("You declined an invitation to " + theEvent.Name, MessageType.SUCCESS);
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
            if (Request.IsAjaxRequest())
            {
                return Json(new { message = TempData["message"] as Message }, JsonRequestBehavior.AllowGet);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult GetSideBar()
        {
            User user = service.GetUser(User.Identity.Name);
            if (Request.IsAjaxRequest())
            {
                EventFeedViewModel model = new EventFeedViewModel();
                model.SideBar = service.GetSideBar(user.UserID, null);
                return Json(RenderPartialViewToString("SideBar", model), JsonRequestBehavior.AllowGet);
            }
            return View("Index");
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