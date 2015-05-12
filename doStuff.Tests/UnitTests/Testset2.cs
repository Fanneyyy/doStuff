using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using doStuff.Databases;
using doStuff.Services;
using doStuff.Models;
using doStuff.Models.DatabaseModels;
using System.Collections.Generic;
using ErrorHandler;
using doStuff.ViewModels;

namespace doStuff.Tests.DbTest
{
    [TestClass]
    public class Testset2
    {
        private Database db;

        [TestInitialize]
        public void Initialize()
        {
            MockDb mock = new MockDb();

            #region Records
            #region User
            mock.Users.Add(new User(true, "Hrafn", "Hrafn Orri Hrafnkelsson", 1994, Gender.MALE, "HrafnOrri1207@gmail.com", 1));
            mock.Users.Add(new User(true, "Gudni", "Gudni Fannar Kristjánsson", 1994, Gender.MALE, "Drulludanni5@gmail.com", 2));
            mock.Users.Add(new User(false, "Kristinn", "Kristinn Þorri Þrastarsson", 1989, Gender.MALE, "Kristinn@gmail.com", 3));
            mock.Users.Add(new User(true, "Helgi", "Helgi Rúnar Einarsson", 1989, Gender.MALE, "Helgi@gmail.com", 4));
            mock.Users.Add(new User(true, "Fanney", "Fanney Sigurðardóttir", 1985, Gender.FEMALE, "Fanneyyy@gmail.com", 5));
            #endregion
            #region Group
            mock.Groups.Add(new Group(true, 1, "Lan Group", 1));
            mock.Groups.Add(new Group(true, 1, "MVC Project", 2));
            mock.Groups.Add(new Group(true, 2, "Bobby Tables", 3));
            mock.Groups.Add(new Group(false, 1, "HearthStone For Live", 4));
            mock.Groups.Add(new Group(true, 2, "Runescape fanatics", 5));
            mock.Groups.Add(new Group(true, 3, "Ghost in the machine", 6));
            mock.Groups.Add(new Group(false, 3, "Ghost birthdays", 7));
            mock.Groups.Add(new Group(false, 4, "Sports for live", 8));
            #endregion
            #region Event
            mock.Events.Add(new Event(true, null, 5, "Sund", "Non", "FAra í sund og fá okkur ís", new DateTime(), new DateTime(), 30, "Kópavogi", 1, 2, 1));
            mock.Events.Add(new Event(true, 8, 3, "Fotbolti", "Non1", "Taka smá fótbolta með Braga", new DateTime(), new DateTime(), 30, "Garðabæ", 1, 2, 2));
            mock.Events.Add(new Event(true, 1, 1, "Lan", "Non2", "Taka nokkra aom heima hjá hrafni", new DateTime(), new DateTime(), 30, "Kópavogi", 1, 2, 3));
            mock.Events.Add(new Event(true, 1, 2, "RuneScape", "Non3", "Mine some ores", new DateTime(), new DateTime(), 30, "Akureyri", 1, 2, 4));
            mock.Events.Add(new Event(true, 1, 2, "League Of Legends", "Non4", "Taka ap darius", new DateTime(), new DateTime(), 30, "Kópavogi", 1, 2, 5));
            mock.Events.Add(new Event(false, 1, 1, "HearthStone", "Non5", "Spila smá aggro warrior", new DateTime(), new DateTime(), 30, "Kópavogi", 1, 2, 6));
            mock.Events.Add(new Event(true, 1, 5, "Heroes of the storm", "Non6", "Feed Lili", new DateTime(), new DateTime(), 30, "Hafnafyrði", 1, 2, 7));
            mock.Events.Add(new Event(true, 8, 4, "Korfubolti", "Non7", "Shoot some hoops", new DateTime(), new DateTime(), 30, "Kópavogi", 1, 2, 8));
            mock.Events.Add(new Event(false, 8, 4, "Golf", "Non8", "Spila 18 holur", new DateTime(), new DateTime(), 30, "Garðabæ", 1, 2, 9));
            mock.Events.Add(new Event(true, 2, 3, "Database", "Non9", "Halda áfram með databasið", new DateTime(), new DateTime(), 30, "Kópavogi", 1, 2, 10));
            #endregion
            #region Comment
            mock.Comments.Add(new Comment(true, 1, "First1", new DateTime(1,1,1), 1));
            mock.Comments.Add(new Comment(true, 1, "First2", new DateTime(1,1,1), 2));
            mock.Comments.Add(new Comment(true, 1, "First3", new DateTime(1,1,1), 3));
            mock.Comments.Add(new Comment(false, 2, "Second", new DateTime(2, 2, 2), 4));
            #endregion
            #endregion
            #region Relations
            #region UserToUser
            mock.UserToUserRelations.Add(new UserToUserRelation(true, 1, 2, true, 1));
            mock.UserToUserRelations.Add(new UserToUserRelation(true, 1, 3, true, 2));
            mock.UserToUserRelations.Add(new UserToUserRelation(true, 1, 4, true, 3));
            mock.UserToUserRelations.Add(new UserToUserRelation(false, 1, 5, false, 4));
            mock.UserToUserRelations.Add(new UserToUserRelation(true, 1, 5, false, 4));
            mock.UserToUserRelations.Add(new UserToUserRelation(true, 2, 3, null, 5));
            #endregion
            #region GroupToUserRelation
            mock.GroupToUserRelations.Add(new GroupToUserRelation(true, 1, 1, 1));
            mock.GroupToUserRelations.Add(new GroupToUserRelation(true, 1, 2, 2));
            mock.GroupToUserRelations.Add(new GroupToUserRelation(true, 1, 5, 2));

            mock.GroupToUserRelations.Add(new GroupToUserRelation(true, 2, 1, 3));
            mock.GroupToUserRelations.Add(new GroupToUserRelation(true, 2, 2, 4));
            mock.GroupToUserRelations.Add(new GroupToUserRelation(true, 2, 3, 5));
            mock.GroupToUserRelations.Add(new GroupToUserRelation(true, 2, 4, 6));
            mock.GroupToUserRelations.Add(new GroupToUserRelation(true, 2, 5, 7));

            mock.GroupToUserRelations.Add(new GroupToUserRelation(true, 3, 1, 8));

            mock.GroupToUserRelations.Add(new GroupToUserRelation(false, 4, 1, 9));

            mock.GroupToUserRelations.Add(new GroupToUserRelation(true, 5, 2, 10));

            mock.GroupToUserRelations.Add(new GroupToUserRelation(true, 6, 3, 11));
            mock.GroupToUserRelations.Add(new GroupToUserRelation(true, 6, 1, 12));
            mock.GroupToUserRelations.Add(new GroupToUserRelation(true, 6, 2, 13));

            mock.GroupToUserRelations.Add(new GroupToUserRelation(false, 7, 2, 14));
            mock.GroupToUserRelations.Add(new GroupToUserRelation(false, 7, 2, 15));
            mock.GroupToUserRelations.Add(new GroupToUserRelation(true, 7, 2, 16));
            #endregion
            #region GroupToEventRelation
            mock.GroupToEventRelations.Add(new GroupToEventRelation(true, 8, 2, 1));
            mock.GroupToEventRelations.Add(new GroupToEventRelation(true, 1, 3, 2));
            mock.GroupToEventRelations.Add(new GroupToEventRelation(true, 1, 4, 3));
            mock.GroupToEventRelations.Add(new GroupToEventRelation(true, 1, 5, 4));
            mock.GroupToEventRelations.Add(new GroupToEventRelation(true, 1, 6, 5));
            mock.GroupToEventRelations.Add(new GroupToEventRelation(true, 1, 7, 6));
            mock.GroupToEventRelations.Add(new GroupToEventRelation(true, 8, 8, 7));
            mock.GroupToEventRelations.Add(new GroupToEventRelation(true, 8, 9, 8));
            mock.GroupToEventRelations.Add(new GroupToEventRelation(false, 2, 10, 1));
            mock.GroupToEventRelations.Add(new GroupToEventRelation(true, 2, 10, 9));
            mock.GroupToEventRelations.Add(new GroupToEventRelation(true, 0, 0, 1));
            mock.GroupToEventRelations.Add(new GroupToEventRelation(true, 1, 1, 1));
            mock.GroupToEventRelations.Add(new GroupToEventRelation(true, 88, 90, 1));
            mock.GroupToEventRelations.Add(new GroupToEventRelation(true, -1, 2, 1));
            #endregion
            #region EventToUserRelation
            mock.EventToUserRelations.Add(new EventToUserRelation(true, 3, 1, true, 1));
            mock.EventToUserRelations.Add(new EventToUserRelation(false, 4, 1, true, 2));
            mock.EventToUserRelations.Add(new EventToUserRelation(true, 4, 1, false, 3));
            mock.EventToUserRelations.Add(new EventToUserRelation(true, 6, 1, true, 4));
            mock.EventToUserRelations.Add(new EventToUserRelation(true, 5, 1, true, 5));

            mock.EventToUserRelations.Add(new EventToUserRelation(true, 1, 2, null, 1));
            mock.EventToUserRelations.Add(new EventToUserRelation(true, 3, 2, true, 1));
            mock.EventToUserRelations.Add(new EventToUserRelation(true, 4, 2, true, 1));
            mock.EventToUserRelations.Add(new EventToUserRelation(true, 5, 2, false, 1));
            mock.EventToUserRelations.Add(new EventToUserRelation(true, 7, 2, true, 1));
            #endregion
            #region EventToCommentRelation
            mock.EventToCommentRelations.Add(new EventToCommentRelation(false, 1, 2, 1));
            mock.EventToCommentRelations.Add(new EventToCommentRelation(true, 1, 2, 2));
            mock.EventToCommentRelations.Add(new EventToCommentRelation(true, 6, 1, 3));
            mock.EventToCommentRelations.Add(new EventToCommentRelation(true, 1, 2, 4));
            mock.EventToCommentRelations.Add(new EventToCommentRelation(true, 1, 2, 5));
            mock.EventToCommentRelations.Add(new EventToCommentRelation(false, 10, 1, 6));
            mock.EventToCommentRelations.Add(new EventToCommentRelation(false, 9, 3, 7));
            mock.EventToCommentRelations.Add(new EventToCommentRelation(false, 9, 4, 8));
            mock.EventToCommentRelations.Add(new EventToCommentRelation(false, 9, 5, 9));
            #endregion
            #endregion

            db = new Database(mock);
        }

        #region DatabaseGetRecordList
        #region GetFriends
                [TestMethod]
                public void DatabaseGetFriends1()
                {
                    List<User> expectedList = new List<User>();
                    expectedList.Add(new User(true, "Gudni", "Gudni Fannar Kristjánsson", 1994, Gender.MALE, "Drulludanni5@gmail.com", 2));
                    expectedList.Add(new User(true, "Helgi", "Helgi Rúnar Einarsson", 1989, Gender.MALE, "Helgi@gmail.com", 4));

                    List<User> actualList = db.GetFriends(1);

                    Assert.AreEqual(expectedList.Count, actualList.Count);

                    for(int i = 0; i < actualList.Count; i++)
                    {
                        Assert.AreEqual(true, Equal(expectedList[i], actualList[i]));
                    }
                }
            #endregion
        #region GetFriendRequests
        #endregion
        #region GetMembers
        #endregion
        #region GetGroups
        #endregion
        #region GetEvents
        #endregion
        #region GetGroupEvents
        #endregion
        #region GetComments
        #endregion
        #endregion

        #region Databse
        #region Exists
        #region User
            [TestMethod]
            public void ExistsUser()
            {
                Assert.IsTrue(db.ExistsUser(1));
                Assert.IsTrue(db.ExistsUser(2));
                Assert.IsFalse(db.ExistsUser(3));
                Assert.IsTrue(db.ExistsUser(4));
                Assert.IsTrue(db.ExistsUser(5));

                for(int i = 6; i < 100; i++)
                {
                    Assert.IsFalse(db.ExistsUser(i));
                }
                for (int i = 0; -100 < i; i--)
                {
                    Assert.IsFalse(db.ExistsUser(i));
                }
            }
        #endregion
        #region Group
        #endregion
        #region Event
        #endregion
        #region Comment
        #endregion
        #endregion
        #endregion

        #region HelperFunctions
        bool Equal(User a, User b)
        {
            if(a == null && b == null)
            {
                return true;
            }
            if (a == null || b == null)
            {
                return false;
            }
            if (a.UserID != b.UserID)
            {
                return false;
            }
            if (a.Active != b.Active)
            {
                return false;
            }
            if (a.UserName != b.UserName)
            {
                return false;
            }
            if (a.DisplayName != b.DisplayName)
            {
                return false;
            }
            if (a.BirthYear != b.BirthYear)
            {
                return false;
            }
            if (a.Gender != b.Gender)
            {
                return false;
            }
            if (a.Email != b.Email)
            {
                return false;
            }
            return true;
        }
        bool Equal(Group a, Group b)
        {
            if (a == null && b == null)
            {
                return true;
            }
            if (a == null || b == null)
            {
                return false;
            }
            if(a.GroupID != b.GroupID)
            {
                return false;
            }
            if(a.Active != b.Active)
            {
                return false;
            }
            if(a.OwnerId != b.OwnerId)
            {
                return false;
            }
            if(a.Name != b.Name)
            {
                return false;
            }
            return true;
        }
        bool Equal(Event a, Event b)
        {
            if (a == null && b == null)
            {
                return true;
            }
            if (a == null || b == null)
            {
                return false;
            }
            if (a.EventID != b.EventID)
            {
                return false;
            }
            if (a.Active != b.Active)
            {
                return false;
            }
            if (a.CreationTime != b.CreationTime)
            {
                return false;
            }
            if (a.Description != b.Description)
            {
                return false;
            }
            if (a.GroupId != b.GroupId)
            {
                return false;
            }
            if (a.Location != b.Location)
            {
                return false;
            }
            if (a.Max != b.Max)
            {
                return false;
            }
            if (a.Min != b.Min)
            {
                return false;
            }
            if (a.Minutes != b.Minutes)
            {
                return false;
            }
            if (a.Name != b.Name)
            {
                return false;
            }
            if (a.OwnerId != b.OwnerId)
            {
                return false;
            }
            if (a.Photo != b.Photo)
            {
                return false;
            }
            if (a.TimeOfEvent != b.TimeOfEvent)
            {
                return false;
            }
            return true;
        }
        bool Equal(Comment a, Comment b)
        {
            if (a == null && b == null)
            {
                return true;
            }
            if (a == null || b == null)
            {
                return false;
            }
            if (a.CommentID != b.CommentID)
            {
                return false;
            }
            if (a.OwnerId != b.OwnerId)
            {
                return false;
            }
            if (a.Active != b.Active)
            {
                return false;
            }
            if (a.CreationTime != b.CreationTime)
            {
                return false;
            }
            if (a.Content != b.Content)
            {
                return false;
            }
            return true;
        }
        #endregion
    }
}
