using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace doStuff.Controllers
{
    public class GroupController : ParentController
    {
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

        [HttpPost]
        public ActionResult CreateGroup()
        {
            //TODO
            return View();
        }

        [HttpPost]
        public ActionResult CreateGroup(FormCollection collection)
        {
            //TODO
            return View();
        }
    }
}