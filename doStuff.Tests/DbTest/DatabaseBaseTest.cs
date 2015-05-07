﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using doStuff.Databases;
using doStuff.Models;
using doStuff.Models.DatabaseModels;
using doStuff.POCOs;

namespace doStuff.Tests.Controllers
{
    [TestClass]
    class DatabaseBaseTest
    {
        private DatabaseBase DbTest;

        [TestInitialize]
        public void Initialize()
        {
            MockDb mock = new MockDb();
            UserTable user1 = new UserTable
            {
                UserTableID = 1,
                Active = true,
                UserName = "Gulli Gurka",
                DisplayName = "xXx$w4gM4$t3r420xXx",
                Age = 9000,
                Gender = Gender.MALE,
                Email = "Gulli$wag@yolo.is"
            };
            mock.Users.Add(user1);
            DbTest = new DatabaseBase(mock);
        }

        [TestMethod]
        public void CheckPerson1()
        {
            const int id = 1;
            const string userName = "Gulli Gurka";
            const string displayName = "xXx$w4gM4$t3r420xXx";
            const int age = 9000;
            const Gender gender = Gender.MALE;
            const string Email = "Gulli$wag@yolo.is";


            UserInfo user = DbTest.GetUser(id);


            Assert.AreEqual(user.Id, id);
            Assert.AreEqual(user.UserName, userName);
            Assert.AreEqual(user.DisplayName, displayName);
            Assert.AreEqual(user.Age, age);
            Assert.AreEqual(user.Gender, gender);
            Assert.AreEqual(user.Email, Email);
        }
    }
}
