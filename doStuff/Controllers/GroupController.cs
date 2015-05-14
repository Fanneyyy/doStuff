﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using doStuff.Models.DatabaseModels;
using doStuff.ViewModels;
using doStuff.Services;
using doStuff.Models;
using CustomErrors;

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
            var service = new Service();
            User user = service.GetUser(User.Identity.Name);
            if (service.IsMemberOfGroup(user.UserID, groupId.Value))
            {
                if (Request.IsAjaxRequest())
                {
                    GroupFeedViewModel model = service.GetGroupFeed(groupId.Value, user.UserID);
                    return Json(RenderPartialViewToString("GroupFeed", model), JsonRequestBehavior.AllowGet);
                }
                SetUserFeedback();
                GroupFeedViewModel feed = service.GetGroupFeed(groupId.Value, service.GetUserId(User.Identity.Name));
                return View(feed);
            }
            return RedirectToAction("Index", "User");
        }

        [HttpPost]
        public ActionResult AddMember(int groupId, string username)
        {
            var service = new Service();
            User user = service.GetUser(User.Identity.Name);
            User member = service.GetUser(username);
            if (service.IsOwnerOfGroup(user.UserID, groupId) == false)
            {
                TempData["message"] = new Message("Only the owner of a group can add a member to it", MessageType.INFORMATION);
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
            else
            {

            }
            if (Request.IsAjaxRequest())
            {
                return Json(new { member = member, message = TempData["message"] as Message }, JsonRequestBehavior.AllowGet);
            }
            return RedirectToAction("Index", new { groupId = groupId });
        }

        [HttpPost]
        public ActionResult RemoveMember(int groupId, int memberId)
        {
            var service = new Service();
            User user = service.GetUser(User.Identity.Name);
            User member = null;
            // Special for if user removes himself, i.e. removes group from the banner
            if (memberId == user.UserID)
            {
                service.RemoveMember(memberId, groupId);
                return RedirectToAction("Index", "User");
            }
            if (service.IsOwnerOfGroup(user.UserID, groupId) == false)
            {
                ViewBag.ErrorMessage = "You are not the owner of this group.";
                return RedirectToAction("Index", "Group", new { groupId = groupId });
            }
            if (service.RemoveMember(memberId, groupId))
            {
                member = service.GetUser(memberId);
                ViewBag.SuccessMessage = member.DisplayName + " has been removed from the group";
                return RedirectToAction("Index", "Group", new { groupId = groupId });
            }
            if (Request.IsAjaxRequest())
            {
                return Json(new { member = member, message = TempData["message"] as Message }, JsonRequestBehavior.AllowGet);
            }
            return RedirectToAction("Index", "User");
        }

        [HttpPost]
        public ActionResult RemoveGroup(int groupId)
        {
            var service = new Service();
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
            var service = new Service();
            User user = service.GetUser(User.Identity.Name);

            if (service.IsMemberOfGroup(user.UserID, groupId) == false)
            {
                return RedirectToAction("Index", "User");
            }
            ViewBag.groupId = groupId;
            return View();
        }

        [HttpPost]
        public ActionResult CreateEvent(int groupId, Event newEvent)
        {
            var service = new Service();
            User user = service.GetUser(User.Identity.Name);

            if (service.IsMemberOfGroup(user.UserID, groupId) == false)
            {
                ModelState.AddModelError("Error", "Either the group doesn't exist or you do not have access to it.");
                return View();
            }
            else if (newEvent.Min.HasValue && newEvent.Max.HasValue && (newEvent.Max.Value < newEvent.Min.Value))
            {
                ModelState.AddModelError("Error", "Max can't be higher than min");
            }
            else if (!service.ValidationOfTimeOfEvent(newEvent))
            {
                ModelState.AddModelError("Time of event", "Date of event is not valid");
            }
            else if (ModelState.IsValid)
            {
                newEvent.CreationTime = DateTime.Now;
                newEvent.OwnerId = user.UserID;
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
            var service = new Service();
            User user = service.GetUser(User.Identity.Name);
            if ((service.IsOwnerOfGroup(user.UserID, groupId) && service.IsEventInGroup(groupId, eventId)) || service.IsOwnerOfEvent(user.UserID, eventId))
            {
                return RedirectToAction("Index", new { groupId = groupId });
            }
            return View();
        }

        [HttpPost]
        public ActionResult Comment(int eventId, string content)
        {
            var service = new Service();
            User user = service.GetUser(User.Identity.Name);
            if (String.IsNullOrEmpty(content))
            {

            }
            else if (service.IsInvitedToEvent(user.UserID, eventId))
            {
                Comment myComment = new Comment();
                myComment.Content = content;
                myComment.Active = true;
                myComment.OwnerId = user.UserID;
                myComment.CreationTime = DateTime.Now;
                if (service.CreateComment(eventId, ref myComment))
                {

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
                return Json(new { id = eventId, message = TempData["message"] as Message }, JsonRequestBehavior.AllowGet);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AnswerEvent(int groupId, int eventId, bool answer)
        {
            var service = new Service();
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

        [HttpPost]
        public ActionResult CreateGroup(string name)
        {
            var service = new Service();
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
                return RedirectToAction("Index", new { groupId = group.GroupID });
            }
            TempData["message"] = new Message("Your group could not be created, please try again later.", MessageType.ERROR);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult GetSideBar(int groupId)
        {
            var service = new Service();
            User user = service.GetUser(User.Identity.Name);
            if (service.IsMemberOfGroup(user.UserID, groupId))
            {
                if (Request.IsAjaxRequest())
                {
                    GroupFeedViewModel model = new GroupFeedViewModel();
                    model.SideBar = service.GetSideBar(user.UserID, groupId);
                    model.Group = service.GetGroupById(groupId);
                    return Json(RenderPartialViewToString("SideBar", model), JsonRequestBehavior.AllowGet);
                }
            }
            return Json(null, JsonRequestBehavior.AllowGet);
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