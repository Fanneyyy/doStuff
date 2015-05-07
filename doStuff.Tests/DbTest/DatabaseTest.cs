using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using doStuff.Databases;
using doStuff.Models;
using doStuff.Models.DatabaseModels;
using doStuff.POCOs;

namespace doStuff.Tests.DbTest
{
    [TestClass]
    public class DatabaseTest
    {
        private DatabaseBase DbTest;

        [TestInitialize]
        public void Initialize()
        {
            MockDb mock = new MockDb();
            UserTable user1 = new UserTable
            {
                Active = true,
                UserName = "testeroni",
                DisplayName = "test",
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
