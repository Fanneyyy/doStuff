using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using doStuff.Databases;
using doStuff.Models;
using doStuff.Models.DatabaseModels;
using System.Collections.Generic;
using System.Diagnostics;

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
                EventToUserRelationID = 1,
                Active = true,
                EventId = 1,
                AttendeeId = 2,
                Answer = null
            };

            EventToUserRelation eventUser3 = new EventToUserRelation
            {
                EventToUserRelationID = 1,
                Active = true,
                EventId = 2,
                AttendeeId = 3,
                Answer = true
            };

            EventToUserRelation eventUser4 = new EventToUserRelation
            {
                EventToUserRelationID = 1,
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
                CommentId = 1

            };

            mock.EventToCommentRelations.Add(eventComment1);
            #endregion

            DbTest = new Database(mock);
        }

        [TestMethod]
        public void CheckPerson1Details()
        {
            const int id = 1;
            const string userName = "Gulli Gurka";
            const string displayName = "xXx$w4gM4$t3r420xXx";
            const int birthYear = 9000;
            const Gender gender = Gender.MALE;
            const string Email = "Gulli$wag@yolo.is";


            User user = DbTest.GetUser(id);


            Assert.AreEqual(id, user.UserID);
            Assert.AreEqual(userName, user.UserName);
            Assert.AreEqual(displayName, user.DisplayName);
            Assert.AreEqual(birthYear, user.BirthYear);
            Assert.AreEqual(gender, user.Gender);
            Assert.AreEqual(Email, user.Email);
        }

        [TestMethod]
        public void CheckFriendships()
        {
            const int user1Id = 1;
            const int user2Id = 2;
            const int user3Id = 3;
            const int user1Amount = 1;
            const int user2Amount = 2;
            const int user3Amount = 1;
            const string user2Name = "Siggi Sulta";


            List<User> user1friends = DbTest.GetFriends(user1Id);
            List<User> user2friends = DbTest.GetFriends(user2Id);
            List<User> user3friends = DbTest.GetFriends(user3Id);


            Assert.AreEqual(user1Amount, user1friends.Count);
            Assert.AreEqual(user2Amount, user2friends.Count);
            Assert.AreEqual(user3Amount, user3friends.Count);

            Assert.AreEqual(user1friends[0].UserName, user2Name);
            Assert.AreEqual(user3friends[0].UserName, user2Name);
        }
    }
}
