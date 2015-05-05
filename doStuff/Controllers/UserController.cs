using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace doStuff.Controllers
{
    public class UserController : ParentController
    {
        [HttpGet]
        public ActionResult Index()
        {
            //TODO
            return View();
        }

        [HttpGet]
        public ActionResult AddFriend()
        {
            //TODO
            return View();
        }

        [HttpPost]
        public ActionResult AddFriend(FormCollection collection)
        {
            //TODO
            return View();
        }

        [HttpGet]
        public ActionResult ViewFriendRequests()
        {
            //TODO
            return View();
        }

        [HttpPost]
        public ActionResult AnswerFriendRequests(uint userId, bool answer)
        {
            //TODO
            return View();
        }

        [HttpPost]
        public ActionResult RemoveFriend(uint friendId)
        {
            //TODO
            return View();
        }

        [HttpGet]
        public ActionResult CreateEvent()
        {
            //TODO
            return View();
        }

        [HttpPost]
        public ActionResult CreateEvent(FormCollection collection)
        {
            //TODO
            return View();
        }

        [HttpPost]
        public ActionResult RemoveEvent(uint eventId)
        {
            //TODO
            return View();
        }

        [HttpPost]
        public ActionResult Comment(uint eventId, FormCollection collection)
        {
            //TODO
            return View();
        }

        [HttpGet]
        public ActionResult ChangeName()
        {
            //TODO
            return View();
        }

        [HttpPost]
        public ActionResult ChangeName(FormCollection collection)
        {
            //TODO
            return View();
        }

        [HttpPost]
        public ActionResult JoinEvent(uint eventId)
        {
            //TODO
            return View();
        }
    }
}