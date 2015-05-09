﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using doStuff.Models.DatabaseModels;
using doStuff.ViewModels;
using doStuff.Services;
using ErrorHandler;

namespace doStuff.Controllers
{
    [Authorize]
    public class GroupController : ParentController
    {
        [HttpGet]
        public ActionResult Index(int groupId)
        {
            // Gets the correct feed for the userId
            GroupFeedViewModel feed = service.GetGroupFeed(groupId, service.GetUserId(User.Identity.Name));
            // Returns the feed to the view
            return View(feed);
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
            if (service.IsOwnerOfGroup(user.UserID, groupId) == false)
            {
                return RedirectToAction("Index", "User");
            }
            return View();
        }

        [HttpPost]
        public ActionResult AddMember(int groupId, User newMember)
        {
            User user = service.GetUser(User.Identity.Name);
            User member = service.GetUser(newMember.UserName);
            if(service.IsOwnerOfGroup(user.UserID, groupId) == false)
            {
                return View("Index", "User");
            }
            if(member == null)
            {
                ModelState.AddModelError("Error", "The username " + newMember.UserName + " could not be found.");
                return View();
            }
            if(User.Identity.Name == member.UserName)
            {
                ModelState.AddModelError("Error", "You are already in the group.");
                return View();
            }
            if(service.IsMemberOfGroup(member.UserID, groupId))
            {
                ModelState.AddModelError("Error", newMember.UserName + " is already in the group.");
                return View();
            }
            if(service.AddMember(member.UserID, groupId))
            {
                ModelState.Clear();
                return View();
            }
            return RedirectToAction("Error", "Something went horribly wrong while processing your request, please try again later.");
        }

        [HttpPost]
        public ActionResult RemoveMember(int groupId, int memberId)
        {
            User user = service.GetUser(User.Identity.Name);

            if (service.IsOwnerOfGroup(user.UserID, groupId) == false)
            {
                return View("Index", "User");
            }

            if (service.RemoveMember(memberId, groupId))
            {
                return View();
            }

            return View();
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
            //TODO
            return View();
        }

        [HttpPost]
        public ActionResult Comment(int groupId, int eventId, Comment newComment)
        {
            //TODO
            service.CreateComment(eventId, newComment);
            return RedirectToAction("Index", new { groupId = groupId });
        }

        [HttpGet]
        public ActionResult ChangeDisplayName(int groupId)
        {
            //TODO
            return View(new { groupId = groupId });
        }

        [HttpPost]
        public ActionResult ChangeDisplayName(int groupId, User myUser)
        {
            //TODO
            int myId = service.GetUserId(User.Identity.Name);
            service.ChangeDisplayName(myId, myUser.DisplayName);
            return RedirectToAction("Index", new { groupId = groupId });
        }

        [HttpPost]
        public ActionResult AnswerEvent(uint groupId, uint EventId, bool answer)
        {
            //TODO
            return View();
        }

        [HttpGet]
        public ActionResult CreateGroup()
        {
            //TODO
            return View();
        }

        [HttpPost]
        public ActionResult CreateGroup(Group newGroup)
        {
            newGroup.Active = true;
            newGroup.OwnerId = service.GetUserId(User.Identity.Name);
            if (service.CreateGroup(newGroup))
            {
                return RedirectToAction("Index", new { groupId = newGroup.GroupID });
            }
            return View();
        }
    }
}