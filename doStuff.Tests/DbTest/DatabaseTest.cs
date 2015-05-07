using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using doStuff.Databases;
using doStuff.Models;
using doStuff.Models.DatabaseModels;

namespace doStuff.Tests.DbTest
{
    [TestClass]
    public class DatabaseTest
    {
        private Database DbTest;

        [TestInitialize]
        public void Initialize()
        {
            MockDb mock = new MockDb();
            User user1 = new User
            {   
                UserID = 1,
                Active = true,
                UserName = "Gulli Gurka",
                DisplayName = "xXx$w4gM4$t3r420xXx",
                BirthYear = 9000,
                Gender = Gender.MALE,
                Email = "Gulli$wag@yolo.is"
            };
            mock.Users.Add(user1);
            DbTest = new Database(mock);
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


            User user = DbTest.GetUser(id);


            Assert.AreEqual(id, user.UserID);
            Assert.AreEqual(userName, user.UserName);
            Assert.AreEqual(user.DisplayName, displayName);
            Assert.AreEqual(user.BirthYear, age);
            Assert.AreEqual(user.Gender, gender);
            Assert.AreEqual(user.Email, Email);
        }
    }
}
