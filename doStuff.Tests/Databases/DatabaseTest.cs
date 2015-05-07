using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using doStuff.Databases;
using doStuff.Models.DatabaseModels;

namespace doStuff.Tests.Databases
{
    [TestClass]
    public class DatabaseTest
    {
        public Database db;

        [TestInitialize]
        public void Initialize()
        {
            db = new Database(new MockDatabaseContext());
            db.CreateUser(new User(true, "Ari1", "Ari Arasson", 1994, Gender.MALE, "HrafnOrri@gmail.com"));
            db.CreateUser(new User(true, "Ari2", "Ari Arasson", 1994, Gender.MALE, "HrafnOrri@gmail.com"));
            db.CreateUser(new User(true, "Ari3", "Ari Arasson", 1994, Gender.MALE, "HrafnOrri@gmail.com"));
            db.CreateUser(new User(true, "Ari4", "Ari Arasson", 1994, Gender.MALE, "HrafnOrri@gmail.com"));
            db.CreateUser(new User(true, "Ari5", "Ari Arasson", 1994, Gender.MALE, "HrafnOrri@gmail.com"));
            db.CreateUser(new User(true, "Ari6", "Ari Arasson", 1994, Gender.MALE, "HrafnOrri@gmail.com")); 
        }

        [TestMethod]
        public void Test()
        {
            User test6 = db.GetUser("Ari6");
            Assert.AreEqual("Ari6", test6.UserName);
        }
    }
}
