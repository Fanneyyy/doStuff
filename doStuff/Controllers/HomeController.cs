﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using doStuff.Services;
using doStuff.Databases;
using doStuff.Models.DatabaseModels;
using System.Diagnostics;

namespace doStuff.Controllers
{
    //[Authorize]
    public class HomeController : ParentController
    {
        private static UserService service = new UserService();
        DatabaseBase Dbase = new DatabaseBase(null);
        DoStuffDatabase tmp = new DoStuffDatabase();
        public ActionResult Index()
        { 
            /*UserTable user = new UserTable();
            user.Email = "Ironpeak";
            db.Users.Add(user);
            db.SaveChanges();*/
            //var ret = (from p in db.Users select p).First();
            User user1 = new User
            {
                //UserID = 1,
                Active = true,
                UserName = "testeroni",
                DisplayName = "test",
                Age = 9000,
                Gender = Gender.MALE,
                Email = "Gulli$wag@yolo.is"
            };
            //DoStuffDatabase.Create();
            Dbase.CreateUser(user1);
            var test = Dbase.GetUser(1);
            Debug.WriteLine(test.UserName);
            return View();
        }
    }
}