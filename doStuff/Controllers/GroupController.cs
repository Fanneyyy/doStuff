using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using doStuff.Models.DatabaseModels;
using doStuff.Services;

namespace doStuff.Controllers
{
    [Authorize]
    public class GroupController : ParentController
    {
        
        static private Service service = new Service();
        [HttpGet]

        public ActionResult Index(uint groupId)
        {
            //TODO
            return View();
        }

        [HttpGet]
        public ActionResult AddMember(uint groupId)
        {
            //TODO
            return View();
        }

        [HttpPost]
        public ActionResult AddMember(uint groupId, FormCollection collection)
        {
            //TODO
            return View();
        }

        [HttpGet]
        public ActionResult RemoveMember(uint groupId, uint memberId)
        {
            //TODO
            return View();
        }

        [HttpPost]
        public ActionResult RemoveGroup(uint groupId)
        {
            //TODO
            return View();
        }

        [HttpPost]
        public ActionResult CreateEvent(uint groupId)
        {
            //TODO
            return View();
        }

        [HttpGet]
        public ActionResult CreateEvent(uint groupId, FormCollection collection)
        {
            //TODO
            return View();
        }

        [HttpPost]
        public ActionResult RemoveEvent(uint groupId, uint eventId)
        {
            //TODO
            return View();
        }

        [HttpPost]
        public ActionResult Comment(uint groupId, uint eventId, FormCollection collection)
        {
            //TODO
            return View();
        }

        [HttpPost]
        public ActionResult ChangeName(uint groupId)
        {
            //TODO
            return View();
        }

        [HttpGet]
        public ActionResult ChangeName(uint groupId, FormCollection collection)
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