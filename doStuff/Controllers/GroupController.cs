using System;
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
        
        static private Service service = new Service();
        [HttpGet]

        public ActionResult Index(int groupId)
        {
            //TODO
            // Gets the correct feed for the userId

            GroupFeedViewModel feed = service.GetGroupFeed(groupId, service.GetUserId(User.Identity.Name));
            // Returns the feed to the view

            return View(feed);
        }

        [HttpGet]
        public ActionResult AddMember(int groupId)
        {
            //TODO
            return View();
        }

        [HttpPost]
        public ActionResult AddMember(int groupId, User newMember)
        {
            //TODO
            try
                {
                    service.AddMember(service.GetUserId(newMember.UserName), groupId);
                }
            catch (UserNotFoundException)
                {
                    return View();
                }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult RemoveMember(int groupId, int memberId)
        {
            //TODO
            return View();
        }

        [HttpPost]
        public ActionResult RemoveGroup(int groupId)
        {
            //TODO
            return View();
        }

        [HttpGet]
        public ActionResult CreateEvent(int groupId)
        {
            //TODO
            return View();
        }

        [HttpPost]
        public ActionResult CreateEvent(int groupId, Event newEvent)
        {
            //TODO
            if (ModelState.IsValid)
            {
                //EventID, GroupID, OwnerId, Name, Photo, Description, CreationTime, TimeOfEvent, Minutes, Location, Min, Max
                newEvent.CreationTime = DateTime.Now;
                newEvent.OwnerId = service.GetUserId(User.Identity.Name);
                newEvent.Minutes = 23;
                newEvent.Active = true;
                newEvent.GroupId = groupId;
                service.CreateEvent(newEvent);
                return RedirectToAction("Index");
            }

            return View(newEvent);
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
            return View();
        }

        [HttpGet]
        public ActionResult ChangeDisplayName(int groupId)
        {
            //TODO
            return View();
        }

        [HttpPost]
        public ActionResult ChangeDisplayName(int groupId, User myUser)
        {
            //TODO
            return View();
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
                return RedirectToAction("Index", "User");
            }
            return View();
        }
    }
}