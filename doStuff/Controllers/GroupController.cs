using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using doStuff.Models.DatabaseModels;
using doStuff.ViewModels;
using doStuff.Services;
using doStuff.Models;
using ErrorHandler;

namespace doStuff.Controllers
{
    //TODO:
    //1. Fix Redirect from commentview to GroupFeed..
    [Authorize]
    public class GroupController : ParentController
    {
        [HttpGet]
        public ActionResult Index(int? groupId)
        {
            if (groupId == null)
            {
                return RedirectToAction("Index", "User");
            }
            User user = service.GetUser(User.Identity.Name);
            if (service.IsMemberOfGroup(user.UserID, groupId.Value))
            {
                SetUserFeedback();
                GroupFeedViewModel feed = service.GetGroupFeed(groupId.Value, service.GetUserId(User.Identity.Name));
                return View(feed);
            }
            return RedirectToAction("Index", "User");
        }

        [HttpGet]
        public ActionResult Banner()
        {
            return RedirectToAction("Index", "User");
        }

        /*[HttpGet]
        public ActionResult AddMember(int groupId)
        {
            User user = service.GetUser(User.Identity.Name);
            if (service.IsOwnerOfGroup(user.UserID, groupId))
            {
                GroupFeedViewModel feed = service.GetGroupFeed(groupId, user.UserID);
                return View(feed);
            }
            return RedirectToAction("Index", "User");
        }*/

        [HttpPost]
        public ActionResult AddMember(int groupId, string username)
        {
            User user = service.GetUser(User.Identity.Name);
            User member = service.GetUser(username);
            if (service.IsOwnerOfGroup(user.UserID, groupId) == false)
            {
                
            }
            else if (member == null)
            {
                TempData["message"] = new Message("The username " + username + " could not be found.", MessageType.INFORMATION);
            }
            else if (User.Identity.Name == member.UserName)
            {
                TempData["message"] = new Message("You are already in the group.", MessageType.INFORMATION);
            }
            else if (service.IsMemberOfGroup(member.UserID, groupId))
            {
                TempData["message"] = new Message(username + " is already in the group.", MessageType.INFORMATION);
            }
            else if (service.AddMember(member.UserID, groupId))
            {
                TempData["message"] = new Message(member.UserName + " was added to the group.", MessageType.SUCCESS);
            }
            return RedirectToAction("Index", new { groupId = groupId });
        }

        [HttpPost]
        public ActionResult RemoveMember(int groupId, int memberId)
        {
            User user = service.GetUser(User.Identity.Name);

            if (service.IsOwnerOfGroup(user.UserID, groupId) == false)
            {
                ViewBag.ErrorMessage = "You are not the owner of this group.";
                return RedirectToAction("Index", "Group", new { groupId = groupId });
            }

            if (service.RemoveMember(memberId, groupId))
            {
                User member = service.GetUser(memberId);
                ViewBag.SuccessMessage = member.DisplayName + " has been removed from the group";
                return RedirectToAction("Index", "Group", new { groupId = groupId });
            }

            return RedirectToAction("Index", "User");
        }

        [HttpPost]
        public ActionResult RemoveGroup(int groupId)
        {
            User user = service.GetUser(User.Identity.Name);

            if (service.IsOwnerOfGroup(user.UserID, groupId) == false)
            {
                return View("Index", "User");
            }

            if (service.RemoveGroup(groupId))
            {
                return RedirectToAction("Index", "User");
            }

            return View();
        }

        [HttpGet]
        public ActionResult CreateEvent(int groupId)
        {
            User user = service.GetUser(User.Identity.Name);

            if (service.IsMemberOfGroup(user.UserID, groupId) == false)
            {
                return RedirectToAction("Index", "User");
            }

            return View();
        }

        [HttpPost]
        public ActionResult CreateEvent(int groupId, Event newEvent)
        {
            User user = service.GetUser(User.Identity.Name);

            if (service.IsMemberOfGroup(user.UserID, groupId) == false)
            {
                ModelState.AddModelError("Error", "Either the group doesn't exist or you do not have access to it.");
                return View();
            }

            if (ModelState.IsValid)
            {
                newEvent.CreationTime = DateTime.Now;
                newEvent.OwnerId = user.UserID;
                newEvent.Minutes = 1;
                newEvent.Min = 2;
                newEvent.Max = 4;
                newEvent.Active = true;
                if (service.CreateEvent(ref newEvent))
                {
                    return RedirectToAction("Index", new { groupId = groupId });
                }
                ModelState.AddModelError("Error", "Something went wrong when trying to add your event to the group, please try again later.");
                return View();
            }

            return View();
        }

        [HttpPost]
        public ActionResult RemoveEvent(int groupId, int eventId)
        {
            User user = service.GetUser(User.Identity.Name);
            if ((service.IsOwnerOfGroup(user.UserID, groupId) && service.IsEventInGroup(groupId, eventId)) || service.IsOwnerOfEvent(user.UserID, eventId))
            {
                return RedirectToAction("Index", new { groupId = groupId });
            }
            return View();
        }
        [HttpGet]
        public ActionResult Comment()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Comment(int eventId, Comment newComment)
        {
            User user = service.GetUser(User.Identity.Name);
            if (service.IsInvitedToEvent(user.UserID, eventId))
            {
                if (ModelState.IsValid)
                {
                    newComment.Active = true;
                    newComment.OwnerId = user.UserID;
                    newComment.CreationTime = DateTime.Now;
                    if (service.CreateComment(eventId, ref newComment))
                    {
                        return RedirectToAction("Index", "User");
                    }
                    ModelState.AddModelError("Error", "Something went wrong when creating your comment, please try again later.");
                    return View();
                }
                return View();
            }
            ModelState.AddModelError("Error", "Either the event doesn't exist or you do not have sufficient access to it.");
            return View();
        }

        [HttpGet]
        public ActionResult ChangeName(int groupId)
        {
            SetUserFeedback();
            return View(new { groupId = groupId });
        }

        [HttpPost]
        public ActionResult ChangeName(int groupId, User myUser)
        {
            User user = service.GetUser(User.Identity.Name);

            if (ModelState.IsValid)
            {
                service.ChangeDisplayName(user.UserID, myUser.DisplayName);
            }

            return RedirectToAction("Index", new { groupId = groupId });
        }

        [HttpPost]
        public ActionResult AnswerEvent(int groupId, int eventId, bool answer)
        {
            User user = service.GetUser(User.Identity.Name);

            if (service.IsInvitedToEvent(user.UserID, eventId))
            {
                if (service.AnswerEvent(user.UserID, eventId, answer))
                {
                    return RedirectToAction("Index", new { groupId = groupId });
                }
                return RedirectToAction("Error", "An error occured when processing your request, please try again later.");
            }

            return RedirectToAction("Error", "Either the event you are trying to access doesn't exist or you do not have sufficient access to it.");
        }

        [HttpGet]
        public ActionResult CreateGroup()
        {
            SetUserFeedback();
            return View();
        }

        [HttpPost]
        public ActionResult CreateGroup(string name)
        {
            User user = service.GetUser(User.Identity.Name);
            if (String.IsNullOrEmpty(name))
            {
                TempData["message"] = new Message("Group name can not be empty.", MessageType.ERROR);
                return RedirectToAction("CreateGroup");
            }
            Group group = new Group(true, user.UserID, name, 0);
            if (service.CreateGroup(ref group))
            {
                TempData["message"] = new Message("Your group was created!", MessageType.SUCCESS);
                return RedirectToAction("CreateGroup");
            }
            TempData["message"] = new Message("Your group could not be created, please try again later.", MessageType.ERROR);
            return RedirectToAction("CreateGroup");
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