﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using doStuff.ViewModels;
using doStuff.Services;
using doStuff.Models;
using doStuff.Models.DatabaseModels;
using doStuff.Databases;
namespace doStuff.Controllers
{
    public class UserController : ParentController
    {
        static Service service = new Service();
        private static Database db = new Database();

        [HttpGet]
        [Authorize]
        public ActionResult Index()
        {/*
            EventFeedViewModel feed = new EventFeedViewModel();
            // Gets userId of the user viewing the site
            int userId = service.GetUserId(User.Identity.Name);
            // Gets the correct feed for the userId
            feed = service.GetEventFeed(userId);
            // Returns the feed to the view

            return View(feed);*/

            User newUser = new User();
            newUser.Age = 25;
            newUser.Active = true;
            newUser.DisplayName = "Svenni hundur";
            newUser.Email = "svennihundur@doggie.is";
            newUser.Gender = Gender.MALE;
            newUser.UserID = 01;
            newUser.UserName = "svennidog";

            Group group1 = new Group(), group2 = new Group();
            group1.Active = true;
            group1.GroupID = 01;
            group1.Name = "Bolti";
            group1.OwnerId = 01;

            group2.Active = true;
            group2.GroupID = 02;
            group2.Name = "Karfa";
            group2.OwnerId = 01;

           /* GroupToUserRelation relation1 = new GroupToUserRelation();
            GroupToUserRelation relation2 = new GroupToUserRelation();

            relation1.GroupId = group1.GroupID;
            relation1.MemberId = newUser.UserID;
            relation1.Active = true;
            relation1.GroupToUserRelationID = 1;

            relation2.GroupId = group2.GroupID;
            relation2.MemberId = newUser.UserID;
            relation2.Active = true;
            relation2.GroupToUserRelationID = 2;
            */
            List<Group> groups = new List<Group>();
            groups.Add(group1);
            groups.Add(group2);

            Event newEvent1 = new Event();
            Event newEvent2 = new Event();
            newEvent1.EventID = 01;
            newEvent1.Description = "Allt ad fretta";
            newEvent1.Active = true;
            newEvent1.CreationTime = new DateTime(2015, 5, 05);
            newEvent1.Location = "heima";
            newEvent1.EventID = 02;
            newEvent1.Description = "Allt ad fretta2";
            newEvent1.Active = true;
            newEvent1.CreationTime = new DateTime(2015, 5, 06);
            newEvent1.Location = "heima hja ther";

            EventFeedViewModel feed = new EventFeedViewModel();

            List<Event> events = new List<Event>();
            events.Add(newEvent1);
            events.Add(newEvent2);

            Comment comment = new Comment();
            comment.Active = true;
            comment.CommentID = 01;
            comment.Content = "YEAHHH!";
            comment.CreationTime = new DateTime(2015, 5, 06);
            comment.OwnerId = 01;

            List<Comment> comments = new List<Comment>();
            comments.Add(comment);
            comments.Add(comment);


            List<EventViewModel> eventViews = new List<EventViewModel>();
            

            foreach (Event eachEvent in events)
            {
                EventViewModel eventView = new EventViewModel();
                eventView.Owner = "svennidog";
                eventView.Event = eachEvent;
                eventView.Comments = comments;
                eventViews.Add(eventView);
            }

            List<User> users = new List<User>();

            SideBarViewModel sidebar = new SideBarViewModel();
            sidebar.User = newUser;
            users.Add(newUser);
            users.Add(newUser);
            sidebar.UserList = users;
            feed.Events = eventViews;
            feed.Groups = groups;
            feed.SideBar = sidebar;
            return View(feed);
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
            return View();
        }

        [HttpPost]
        public ActionResult CreateEvent(/*[Bind(Include = "Id,Title,Text,DateCreated,Category")], */Event newEvent)
        {
            if (ModelState.IsValid)
            {
                service.CreateEvent(newEvent);
                return RedirectToAction("Index");
            }

            return View(newEvent);
        }

        [HttpPost]
        public ActionResult RemoveEvent(int eventId)
        {
            //TODO
            return View();
        }

        [HttpPost]
        public ActionResult Comment(int eventId, FormCollection collection)
        {
            //TODO
            return View();
        }

        [HttpGet]
        public ActionResult ChangeUserName()
        {
            //TODO

            return View();
        }

        [HttpPost]
        public ActionResult ChangeUserName(Event eventToChange)
        {
            //TODO

            return View();
        }

        [HttpPost]
        public ActionResult AnswerEvent(int eventId, bool answer)
        {
            //TODO
            return View();
        }
    }
}