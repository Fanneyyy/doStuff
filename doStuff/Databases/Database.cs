﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using doStuff.Models.DatabaseModels;

namespace doStuff.Databases
{
    public class Database
    {
        protected static IDataContext db;

        public Database(IDataContext dbContext)
        {
            db = dbContext ?? new DatabaseContext();       
        }

        #region GetRecordLists
        public List<User> GetFriends(int userId)
        {
            List<User> friends = new List<User>();

            List<int> friendIds = (from f in db.UserToUserRelations
                                   where f.ReceiverId == userId && f.Active == true
                                   select f.SenderId).ToList();

            friendIds = friendIds.Concat(from f in db.UserToUserRelations
                                         where f.SenderId == userId && f.Active == true
                                         select f.ReceiverId).ToList();

            foreach(var id in friendIds)
            {
                friends.Add(GetUser(id));
            }

            return friends;
        }
        public List<User> GetMembers(int groupId)
        {
            List<User> members = new List<User>();

            List<int> memberIds = (from g in db.GroupToUserRelations
                                   where g.GroupId == groupId && g.Active == true
                                   select g.MemberId).ToList();

            foreach(var id in memberIds)
            {
                User user = (from u in db.Users
                                  where u.UserID == id && u.Active == true
                                  select u).SingleOrDefault();
                if (user != null)
                {
                    members.Add(user);
                }
            }

            return members;
        }
        public List<Group> GetGroups(int userId)
        {
            List<Group> groups = new List<Group>();

            var groupIDs = (from g in db.GroupToUserRelations
                            where g.MemberId == userId && g.Active == true
                            select g.GroupId);

            foreach(var id in groupIDs)
            {
                Group group = (from g in db.Groups
                                    where g.GroupID == id && g.Active == true
                                    select g).SingleOrDefault();
                if (group != null)
                {
                    groups.Add(group);
                }
            }
            return groups;
        }
        public List<Event> GetEvents(int userId)
        {
            List<Event> events = (from e in db.Events
                                      where e.OwnerId == userId && e.GroupId == null && e.Active == true
                                      select e).ToList();
            return events;
        }
        public List<Event> GetGroupEvents(int groupId)
        {
            List<Event> events = (from e in db.Events
                                      where e.GroupId == groupId && e.Active == true
                                      select e).ToList();
            return events;
        }
        public List<Comment> GetComments(int eventId)
        {
            List<Comment> comments = new List<Comment>();

            var commentIDs = (from c in db.EventToCommentRelations
                              where c.EventId == eventId && c.Active == true
                              select c.EventId);

            foreach (var id in commentIDs)
            {
                Comment comment = (from c in db.Comments where c.CommentID == id && c.Active == true select c).SingleOrDefault();

                if (comment != null)
                {
                    comments.Add(comment);
                }
            }
            return comments;
        }
        #endregion
        #region RecordTables
            #region Exists
            public bool ExistsUser(int userId)
            {
                return (1 == (from u in db.Users where u.UserID == userId && u.Active == true select u).Count());
            }
            public bool ExistsGroup(int groupId)
            {
                return (1 == (from g in db.Groups where g.GroupID == groupId && g.Active == true select g).Count());
            }
            public bool ExistsEvent(int eventId)
            {
                return (1 == (from e in db.Events where e.EventID == eventId && e.Active == true select e).Count());
            }
            public bool ExistsComment(int commentId)
            {
                return (1 == (from c in db.Comments where c.CommentID == commentId && c.Active == true select c).Count());
            }
            #endregion
            #region Create
            public bool CreateUser(User user)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return true;
            }
            public bool CreateGroup(Group group)
            {
                db.Groups.Add(group);
                db.SaveChanges();
                return true;
            }
            public bool CreateEvent(Event newEvent)
            {
                db.Events.Add(newEvent);
                db.SaveChanges();
                return true;
            }
            public bool CreateComment(Comment comment)
            {
                db.Comments.Add(comment);
                db.SaveChanges();
                return false;
            }
            #endregion
            #region Remove
            public bool RemoveUser(int userId)
            {
                var user = (from u in db.Users
                            where u.UserID == userId
                            select u).Single();
                user.Active = false;
                db.SaveChanges();
                return false;
            }
            public bool RemoveGroup(int groupId)
            {
                var group = (from g in db.Groups
                                where g.GroupID == groupId
                                select g).Single();
                group.Active = false;
                db.SaveChanges();
                return false;
            }
            public bool RemoveEvent(int eventId)
            {
                var theEvent = (from e in db.Events
                                where e.EventID == eventId
                                select e).Single();
                theEvent.Active = false;
                db.SaveChanges();
                return false;
            }
            public bool RemoveComment(int commentId)
            {
                var comment = (from c in db.Comments
                                where c.CommentID == commentId
                                select c).Single();
                comment.Active = false;
                db.SaveChanges();
                return false;
            }
            #endregion
            #region Get
            public User GetUser(int userId)
            {
                User user = (from u in db.Users
                                  where u.UserID == userId
                                  select u).Single();
                return user;
            }

            public User GetUser(string userName)
            {
                User user = (from u in db.Users
                                  where u.UserName == userName
                                  select u).Single();
                return user;
            }

            public Group GetGroup(int groupId)
            {
                Group group = (from g in db.Groups
                                    where g.GroupID == groupId
                                    select g).Single();
                return group;
            }

            public Event GetEvent(int eventId)
            {
                Event theEvent = (from e in db.Events
                                       where e.EventID == eventId
                                       select e).Single();
                return theEvent;
            }

            public Comment GetComment(int commentId)
            {
                Comment comment = (from c in db.Comments
                                        where c.CommentID == commentId
                                        select c).Single();
                return comment;
            }
            #endregion
            #region Set
            public bool SetUser(User user)
            {
                var userTable = (from u in db.Users
                                 where u.UserID == user.UserID
                                 select u).Single();
                userTable = user;
                db.SaveChanges();
                return true;
            }
            public bool SetGroup(Group group)
            {
                int groupId = group.GroupID;
                var groupTable = (from g in db.Groups
                                  where g.GroupID == groupId
                                  select g).Single();
                groupTable = group;
                db.SaveChanges();
                return true;
            }
            public bool SetEvent(Event newEvent)
            {
                var eventTable = (from e in db.Events
                                where e.EventID == newEvent.EventID
                                select e).Single();
                eventTable = newEvent;
                db.SaveChanges();
                return true;
            }
            public bool SetComment(Comment comment)
            {
                var commentTable = (from c in db.Comments
                               where c.CommentID == comment.CommentID
                               select c).Single();
                commentTable = comment;
                db.SaveChanges();
                return true;
            }
            #endregion
        #endregion
        #region RelationTable
            #region Exists
            public bool ExistsUserToUserRelation(int senderId, int receiverId)
            {
                var relation = (from r in db.UserToUserRelations
                                where r.SenderId == senderId && r.ReceiverId == receiverId && r.Active == true
                                select r).SingleOrDefault();
                return relation != null;
            }

            public bool ExistsGroupToUserRelation(int groupId, int userId)
            {
                var relation = (from r in db.GroupToUserRelations
                                where r.GroupId == groupId && r.MemberId == userId && r.Active == true
                                select r).SingleOrDefault();
                return relation != null;
            }

            public bool ExistsEventToUserRelation(int eventId, int userId)
            {
                var relation = (from r in db.EventToUserRelations
                                where r.EventId == eventId && r.AttendeeId == userId && r.Active == true
                                select r).SingleOrDefault();
                return relation != null;
            }

            public bool ExistsGroupToEventRelation(int groupId, int eventId)
            {
                var relation = (from r in db.GroupToEventRelations
                                where r.GroupId == groupId && r.EventId == eventId && r.Active == true
                                select r).SingleOrDefault();
                return relation != null;
            }

            public bool ExistsEventToCommentRelation(int eventId, int commentId)
            {
                var relation = (from r in db.EventToCommentRelations
                                where r.EventId == eventId && r.CommentId == commentId && r.Active == true
                                select r).SingleOrDefault();
                return relation != null;
            }
            #endregion
            #region Create
            public bool CreateUserToUserRelation(int senderId, int receiverId)
            {
                UserToUserRelation table = new UserToUserRelation();
                table.Active = true;
                table.SenderId = senderId;
                table.ReceiverId = receiverId;
                table.Answer = false;
                db.UserToUserRelations.Add(table);
                db.SaveChanges();
                return true;
            }

            public bool CreateGroupToUserRelation(int groupId, int userId)
            {
                GroupToUserRelation table = new GroupToUserRelation();
                table.Active = true;
                table.GroupId = groupId;
                table.MemberId = userId;
                db.GroupToUserRelations.Add(table);
                db.SaveChanges();
                return true;
            }
            
            public bool CreateEventToUserRelation(int eventId, int userId)
            {
                EventToUserRelation table = new EventToUserRelation();
                table.Active = true;
                table.EventId = eventId;
                table.AttendeeId = userId;
                table.Answer = null;
                db.EventToUserRelations.Add(table);
                db.SaveChanges();
                return true;
            }

            public bool CreateGroupToEventRelation(int groupId, int EventId)
            {
                GroupToEventRelation table = new GroupToEventRelation();
                table.Active = true;
                table.GroupId = groupId;
                table.EventId = EventId;
                db.GroupToEventRelations.Add(table);
                db.SaveChanges();
                return true;
            }

            public bool CreateEventToCommentRelation(int eventId, int commentId)
            {
                EventToCommentRelation table = new EventToCommentRelation();
                table.Active = true;
                table.EventId = eventId;
                table.CommentId = commentId;
                db.EventToCommentRelations.Add(table);
                db.SaveChanges();
                return true;
            }
            #endregion
            #region Remove
            public bool RemoveUserToUserRelation(int id)
            {
                var table = (from t in db.UserToUserRelations
                             where t.UserToUserRelationID == id
                             select t).Single();
                table.Active = false;
                db.SaveChanges();
                return true;
            }

            public bool RemoveGroupToUserRelation(int id)
            {
                var table = (from t in db.GroupToUserRelations
                             where t.GroupToUserRelationID == id
                             select t).Single();
                table.Active = false;
                db.SaveChanges();
                return true;
            }

            public bool RemoveEventToUserRelation(int id)
            {
                var table = (from t in db.EventToUserRelations
                             where t.EventToUserRelationID == id
                             select t).Single();
                table.Active = false;
                db.SaveChanges();
                return true;
            }

            public bool RemoveGroupToEventRelation(int id)
            {
                var table = (from t in db.GroupToEventRelations
                             where t.GroupToEventRelationID == id
                             select t).Single();
                table.Active = false;
                db.SaveChanges();
                return true;
            }

            public bool RemoveEventToCommentRelation(int id)
            {
                var table = (from t in db.EventToCommentRelations
                             where t.EventToCommentRelationID == id
                             select t).Single();
                table.Active = false;
                db.SaveChanges();
                return true;
            }
            #endregion
            #region Get
            public UserToUserRelation GetUserToUserRelation(int senderId, int receiverId)
            {
                var table = (from t in db.UserToUserRelations
                             where t.SenderId == senderId && t.ReceiverId == receiverId && t.Active == true
                             select t).Single();
                return table;
            }
            public GroupToUserRelation GetGroupToUserRelation(int groupId, int userId)
            {
                var table = (from t in db.GroupToUserRelations
                             where t.GroupId == groupId && t.MemberId == userId && t.Active == true
                             select t).Single();
                return table;
            }

            public EventToUserRelation GetEventToUserRelation(int eventId, int userId)
            {
                var table = (from t in db.EventToUserRelations
                             where t.EventId == eventId && t.AttendeeId == userId && t.Active == true
                             select t).Single();
                return table;
            }

            public GroupToEventRelation GetGroupToEventRelation(int groupId, int eventId)
            {
                var table = (from t in db.GroupToEventRelations
                             where t.GroupId == groupId && t.EventId == eventId && t.Active == true
                             select t).Single();
                return table;
            }
            public EventToCommentRelation GetEventToCommentRelation(int eventId, int commentId)
            {
                var table = (from t in db.EventToCommentRelations
                             where t.EventId == eventId && t.CommentId == commentId && t.Active == true
                             select t).Single();
                return table;
            }
            #endregion    
            #region Set
            public bool SetUserToUserRelation(UserToUserRelation value)
            {
                var table = (from t in db.UserToUserRelations
                             where t.UserToUserRelationID == value.UserToUserRelationID
                             select t).Single();
                table = value;
                db.SaveChanges();
                return true;
            }
            public bool SetGroupToUserRelation(GroupToUserRelation value)
            {
                var table = (from t in db.GroupToUserRelations
                             where t.GroupToUserRelationID == value.GroupToUserRelationID
                             select t).Single();
                table = value;
                db.SaveChanges();
                return true;
            }

            public bool SetEventToUserRelation(EventToUserRelation value)
            {
                var table = (from t in db.EventToUserRelations
                             where t.EventToUserRelationID == value.EventToUserRelationID
                             select t).Single();
                table = value;
                db.SaveChanges();
                return true;
            }

            public bool SetGroupToEventRelation(GroupToEventRelation value)
            {
                var table = (from t in db.GroupToEventRelations
                             where t.GroupToEventRelationID == value.GroupToEventRelationID
                             select t).Single();
                table = value;
                db.SaveChanges();
                return true;
            }

            public bool SetEventToCommentRelation(EventToCommentRelation value)
            {
                var table = (from t in db.EventToCommentRelations
                             where t.EventToCommentRelationID == value.EventToCommentRelationID
                             select t).Single();
                table = value;
                db.SaveChanges();
                return true;
            }
            #endregion
        #endregion
    }
}