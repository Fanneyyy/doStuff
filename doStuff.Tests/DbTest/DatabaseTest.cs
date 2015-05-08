﻿using System;
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
    public class DatabaseTest
    {
        private Database DbTest;
        private Service ServiceTest;
        [TestInitialize]
        public void Initialize()
        {

            MockDb mock = new MockDb();
        
            #region Users
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
            User user2 = new User
            {
                UserID = 2,
                Active = true,
                UserName = "Siggi Sulta",
                DisplayName = "Sultumenni500",
                BirthYear = 1337,
                Gender = Gender.MALE,
                Email = "maxyolo@420.is"
            };
            User user3 = new User
            {
                UserID = 3,
                Active = true,
                UserName = "Halli Megapulsa",
                DisplayName = "Pulsmeister99",
                BirthYear = 9000,
                Gender = Gender.FEMALE,
                Email = "pullipuls@urmom.is"
            };
            mock.Users.Add(user1);
            mock.Users.Add(user2);
            mock.Users.Add(user3);
            #endregion
            #region Groups

            Group group1 = new Group
            {
                GroupID = 1,
                Active = true,
                OwnerId = 1,
                Name = "Team Gulli"
            }; 
            Group group2 = new Group
            {
                GroupID = 2,
                Active = true,
                OwnerId = 3,
                Name = "Pulsuvagninn"
            };
            mock.Groups.Add(group1);
            mock.Groups.Add(group2);
            #endregion
            #region Events
            Event event1 = new Event
            {
                EventID = 1,
                Active = true,
                GroupId = null,
                OwnerId = 1,
                Name = "Lan",
                Photo = "",
                Description = "Quake 4 lyfe",
                CreationTime = new DateTime(2015, 5, 6, 12, 30, 1),
                TimeOfEvent = new DateTime(2015, 5, 9, 12, 30, 0),
                Minutes = 60,
                Location = "the internet",
                Min = 0,
                Max = 5,
            }; 
            
            Event event2 = new Event
            {
                EventID = 2,
                Active = true,
                GroupId = 2,
                OwnerId = 3,
                Name = "Pulsuparty",
                Photo = "",
                Description = "pulsulíf",
                CreationTime = new DateTime(2015, 5, 6, 12, 30, 1),
                TimeOfEvent = new DateTime(2015, 5, 9, 12, 30, 0),
                Minutes = 60,
                Location = "bæjarins bestu",
                Min = 0,
                Max = 5,
            };
            mock.Events.Add(event1);
            mock.Events.Add(event2);

            #endregion
            #region Comments
            Comment comment1 = new Comment
            {
                CommentID = 1,
                Active = true,
                OwnerId = 2,
                Content = "FOKK PULSUR, SULTA IS SUPERIOR",
                CreationTime = new DateTime(2015, 5, 6, 12, 35, 1)
            };
            mock.Comments.Add(comment1);

            #endregion
            #region GroupToUserRelations
            GroupToUserRelation groupUser1 = new GroupToUserRelation
            {
                GroupToUserRelationID = 1,
                Active = true,
                GroupId = 1,
                MemberId = 1
            };

            GroupToUserRelation groupUser2 = new GroupToUserRelation
            {
                GroupToUserRelationID = 2,
                Active = true,
                GroupId = 2,
                MemberId = 3
            };

            GroupToUserRelation groupUser3 = new GroupToUserRelation
            {
                GroupToUserRelationID = 3,
                Active = true,
                GroupId = 2,
                MemberId = 2
            };

            GroupToUserRelation groupUser4 = new GroupToUserRelation
            {
                GroupToUserRelationID = 4,
                Active = true,
                GroupId = 1,
                MemberId = 2
            };
            mock.GroupToUserRelations.Add(groupUser1);
            mock.GroupToUserRelations.Add(groupUser2);
            mock.GroupToUserRelations.Add(groupUser3);
            mock.GroupToUserRelations.Add(groupUser4);

            #endregion
            #region GroupsToEventRelations
            GroupToEventRelation groupEvent1 = new GroupToEventRelation 
            {
                GroupToEventRelationID = 1,
                Active = true,
                EventId = 2,
                GroupId = 2
            };
            mock.GroupToEventRelations.Add(groupEvent1);

            #endregion
            #region UserToUserRelations

            UserToUserRelation friendship1 = new UserToUserRelation
            {
                UserToUserRelationID = 1,
                Active = true,
                SenderId = 2,
                ReceiverId = 3,
                Answer = true
            };

            UserToUserRelation friendship2 = new UserToUserRelation
            {
                UserToUserRelationID = 2,
                Active = true,
                SenderId = 1,
                ReceiverId = 2,
                Answer = true
            };

            UserToUserRelation friendship3 = new UserToUserRelation
            {
                UserToUserRelationID = 3,
                Active = true,
                SenderId = 1,
                ReceiverId = 3,
                Answer = null
            };

            mock.UserToUserRelations.Add(friendship1);
            mock.UserToUserRelations.Add(friendship2);
            mock.UserToUserRelations.Add(friendship3);

            #endregion
            #region EventToUserRelations

            EventToUserRelation eventUser1 = new EventToUserRelation
            {
                EventToUserRelationID = 1,
                Active = true,
                EventId = 1,
                AttendeeId = 1,
                Answer = true
            };

            EventToUserRelation eventUser2 = new EventToUserRelation
            {
                EventToUserRelationID = 2,
                Active = true,
                EventId = 1,
                AttendeeId = 2,
                Answer = null
            };

            EventToUserRelation eventUser3 = new EventToUserRelation
            {
                EventToUserRelationID = 3,
                Active = true,
                EventId = 2,
                AttendeeId = 3,
                Answer = true
            };

            EventToUserRelation eventUser4 = new EventToUserRelation
            {
                EventToUserRelationID = 4,
                Active = true,
                EventId = 2,
                AttendeeId = 2,
                Answer = false
            };
            
            mock.EventToUserRelations.Add(eventUser1);
            mock.EventToUserRelations.Add(eventUser2);
            mock.EventToUserRelations.Add(eventUser3);
            mock.EventToUserRelations.Add(eventUser4);

            #endregion
            #region EventToCommentRelations

            EventToCommentRelation eventComment1 = new EventToCommentRelation
            {
                EventToCommentRelationID = 1,
                EventId = 2,
                CommentId = 1,
                Active = true

            };

            mock.EventToCommentRelations.Add(eventComment1);
            #endregion

            DbTest = new Database(mock);
            ServiceTest = new Service(DbTest);
        }
        #region Database tests
        [TestMethod]
        public void DatabaseCheckPerson1()
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

        [TestMethod]
        public void DatabaseGetGroup()
        {
            const int group1Id = 1;
            const int group2Id = 2;
            const string group1Name = "Team Gulli";
            const string group2Name = "Pulsuvagninn";

            Group group1 = DbTest.GetGroup(group1Id);
            Group group2 = DbTest.GetGroup(group2Id);

            Assert.AreEqual(group1Name, group1.Name);
            Assert.AreEqual(group2Name, group2.Name);
        }

        [TestMethod]
        public void DatabaseGetGroups()
        {
            const int user1Id = 1;
            const int user2Id = 2;
            const int user3Id = 3;
            const int user1GroupAmount = 1;
            const int user2GroupAmount = 2;
            const int user3GroupAmount = 1;

            List<Group> user1Groups = DbTest.GetGroups(user1Id);
            List<Group> user2Groups = DbTest.GetGroups(user2Id);
            List<Group> user3Groups = DbTest.GetGroups(user3Id);
                    
            Assert.AreEqual(user1GroupAmount, user1Groups.Count);
            Assert.AreEqual(user2GroupAmount, user2Groups.Count);
            Assert.AreEqual(user3GroupAmount, user3Groups.Count);
        }
        
        [TestMethod]
        public void DatabaseGetGroupEvents()
        {
            const int group1Id = 1;
            const int group2Id = 2;
            const int group1EventsAmount = 0;
            const int group2EventsAmount = 1;
            const string group2Event1Name = "Pulsuparty";

            List<Event> group1Eventslist = DbTest.GetGroupEvents(group1Id);
            List<Event> group2Eventslist = DbTest.GetGroupEvents(group2Id);

            Assert.AreEqual(group1EventsAmount, group1Eventslist.Count);
            Assert.AreEqual(group2EventsAmount, group2Eventslist.Count);
            Assert.AreEqual(group2Event1Name, group2Eventslist[0].Name);
        }

        [TestMethod]
        public void DatabaseGetMembers()
        {
            const int group1Id = 1;
            const int group2Id = 2;
            const int group1MembersAmount = 2;
            const int group2MembersAmount = 2;

            List<User> group1Users = DbTest.GetMembers(group1Id);
            List<User> group2Users = DbTest.GetMembers(group2Id);

            Assert.AreEqual(group1MembersAmount, group1Users.Count);
            Assert.AreEqual(group2MembersAmount, group2Users.Count);
        }

        [TestMethod]
        public void DatabaseGetEvent()
        {
            const int event1Id = 1;
            const int event2Id = 2;
            const string event1Name = "Lan";
            const string event2Name = "Pulsuparty";


            Event event1 = DbTest.GetEvent(event1Id);
            Event event2 = DbTest.GetEvent(event2Id);

            Assert.AreEqual(event1Name, event1.Name);
            Assert.AreEqual(event2Name, event2.Name);
        }
        #endregion
        #region Service tests
        [TestMethod]
        public void ServiceChangeDisplayName()
        {
            const int user1Id = 1;
            const int noUserId = 999;
            const string newName = "Gulli G";


            bool success = ServiceTest.ChangeDisplayName(user1Id, newName);
            User userWithNewName = DbTest.GetUser(user1Id);


            Assert.AreEqual(true, success);
            try
            {
                ServiceTest.ChangeDisplayName(noUserId, newName);
                Assert.Fail();
            }
            catch (ErrorHandler.UserNotFoundException) { }
            Assert.AreEqual(newName, userWithNewName.DisplayName);

        }
        [TestMethod]
        public void ServiceChangeGroupName()
        {
            const int group1Id = 1;
            const int noGroupId = 999;
            const string newName = "Team Gulli & co";

            bool success = ServiceTest.ChangeGroupName(group1Id, newName);
            Group groupWithNewName = DbTest.GetGroup(group1Id);


            Assert.AreEqual(true, success);
            try
            {
                ServiceTest.ChangeGroupName(noGroupId, newName);
                Assert.Fail();
            }
            catch (ErrorHandler.GroupNotFoundException) { }
            Assert.AreEqual(newName, groupWithNewName.Name);
        }
        [TestMethod]
        public void ServiceSendFriendRequest()
        {
            const int user1Id = 1;
            const int user2Id = 3;

            ServiceTest.SendFriendRequest(user1Id, user2Id);
            bool fail1 = ServiceTest.IsFriendsWith(user1Id, user2Id);
            bool fail2 = ServiceTest.IsFriendsWith(user2Id, user1Id);            
            bool fail3 = ServiceTest.AnswerFriendRequest(user1Id, user2Id, true);
            bool fail4 = ServiceTest.IsFriendsWith(user1Id, user2Id);
            bool success1 = ServiceTest.SendFriendRequest(user2Id, user1Id);
            bool success2 = ServiceTest.IsFriendsWith(user1Id, user2Id);
            bool success3 = ServiceTest.IsFriendsWith(user2Id, user1Id);

            Assert.AreEqual(false, fail1);
            Assert.AreEqual(false, fail2);
            Assert.AreEqual(false, fail3);
            Assert.AreEqual(false, fail4);
            Assert.AreEqual(true, success1);
            Assert.AreEqual(true, success2);
            Assert.AreEqual(true, success3);

        }
        [TestMethod]
        public void ServiceCreateComment()
        {
            const int event1Id = 1;
            const int event2Id = 2;
            const int event1CommentAmount = 1;
            const int event2CommentAmount = 2;
            Comment newComment1 = new Comment
            {
                CommentID = 2,
                Content = "ut2k4 lets go bois",
                Active = true,
                OwnerId = 2,
                CreationTime = new DateTime(2015, 5, 6, 16, 35, 1)
            };
            Comment newComment2 = new Comment
            {
                CommentID = 3,
                Content = "Pulsur eru fyrir aula",
                Active = true,
                OwnerId = 2,
                CreationTime = new DateTime(2015, 5, 6, 16, 35, 1)
            };

            ServiceTest.CreateComment(event1Id, newComment1);
            ServiceTest.CreateComment(event2Id, newComment2);
            List<Comment> commentsEvent1 = DbTest.GetComments(event1Id);
            List<Comment> commentsEvent2 = DbTest.GetComments(event2Id);


            Assert.AreEqual(event1CommentAmount, commentsEvent1.Count);
            Assert.AreEqual(event2CommentAmount, commentsEvent2.Count);

        }


        [TestMethod]
        public void TemplateTest()
        {

        }

        #endregion
    }
}
