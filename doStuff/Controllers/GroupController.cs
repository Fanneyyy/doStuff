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
            if(groupId == null)
            {
                return RedirectToAction("Index", "User");
            }
            User user = service.GetUser(User.Identity.Name);
            if(service.IsMemberOfGroup(user.UserID, groupId.Value))
            {
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

        [HttpGet]
        public ActionResult AddMember(int groupId)
        {
            User user = service.GetUser(User.Identity.Name);
            if (service.IsOwnerOfGroup(user.UserID, groupId))
            {
                GroupFeedViewModel feed = service.GetGroupFeed(groupId, user.UserID);
                return View(feed);
            }
            return RedirectToAction("Index", "User");
        }

        [HttpPost]
        public ActionResult AddMember(int groupId, string username)
        {
            User user = service.GetUser(User.Identity.Name);
            User member = service.GetUser(username);
            if(service.IsOwnerOfGroup(user.UserID, groupId) == false)
            {
                return View("Index", "User");
            }
            if(member == null)
            {
                GroupFeedViewModel feed = service.GetGroupFeed(groupId, user.UserID);
                ViewBag.ErrorMessage = "The username " + username + " could not be found.";
                return View(feed);
            }
            if(User.Identity.Name == member.UserName)
            {
                GroupFeedViewModel feed = service.GetGroupFeed(groupId, user.UserID);
                ViewBag.ErrorMessage = "You are already in the group.";
                return View(feed);
            }
            if(service.IsMemberOfGroup(member.UserID, groupId))
            {
                GroupFeedViewModel feed = service.GetGroupFeed(groupId, user.UserID);
                ViewBag.ErrorMessage = username + " is already in the group.";
                return View(feed);
            }
            if(service.AddMember(member.UserID, groupId))
            {
                GroupFeedViewModel feed = service.GetGroupFeed(groupId, user.UserID);
                ViewBag.SuccessMessage = member.UserName + " was added to the group.";
                return View("Index", feed);
            }
            return RedirectToAction("Error", "Something went horribly wrong while processing your request, please try again later.");
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
                newEvent.Minutes = 23;
                newEvent.Min = 2;
                newEvent.Max = 4;
                newEvent.Active = true;
                if (service.CreateEvent(newEvent))
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
            if((service.IsOwnerOfGroup(user.UserID, groupId) && service.IsEventInGroup(groupId, eventId)) || service.IsOwnerOfEvent(user.UserID, eventId))
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
            if(service.IsInvitedToEvent(user.UserID, eventId))
            {
                if(ModelState.IsValid)
                {
                    newComment.Active = true;
                    newComment.OwnerId = user.UserID;
                    newComment.CreationTime = DateTime.Now;
                    if(service.CreateComment(eventId, newComment))
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
            //TODO
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
                    return RedirectToAction("Index", groupId);
                }
                return RedirectToAction("Error", "An error occured when processing your request, please try again later.");
            }

            return RedirectToAction("Error", "Either the event you are trying to access doesn't exist or you do not have sufficient access to it.");
        }

        [HttpGet]
        public ActionResult CreateGroup(Message message = null)
        {
            SetUserFeedback(message);
            return View();
        }

        [HttpPost]
        public ActionResult CreateGroup(Group newGroup)
        {
            User user = service.GetUser(User.Identity.Name);
            Message message;
            if (newGroup.Name == null)
            {
                message = new Message("Enter a Groupname please... Dick.", MessageType.ERROR);
                return View();
            }
            newGroup.Active = true;
            newGroup.OwnerId = user.UserID;
            if (service.CreateGroup(newGroup))
            {
                return RedirectToAction("Index", new { groupId = newGroup.GroupID });
            }
            return View();
        }
        private void SetUserFeedback(Message message)
        {
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