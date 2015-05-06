using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using doStuff.POCOs;
using doStuff.Models.DatabaseModels;

namespace doStuff.Databases
{
    public class DatabaseBase
    {
        protected static DoStuffDatabase db = new DoStuffDatabase();
        #region GetRecordLists
        public List<UserInfo> GetFriends(int userId)
        {
            return null;
        }

        public List<UserInfo> GetMembers(int groupId)
        {
            return null;
        }

        public List<GroupInfo> GetGroups(int userId)
        {
            List<GroupInfo> groups = new List<GroupInfo>();

            var groupIDs = (from g in db.GroupToUserRelations
                            where g.MemberId == userId
                            select g.GroupId);

            foreach(var id in groupIDs)
            {
                groups.Add(TableToEntity((from g in db.Groups where g.GroupTableID == id select g).Single()));
            }
            return groups;
        }

        public List<EventInfo> GetEvents(int userId)
        {
            return null;
        }

        public List<EventInfo> GetGroupEvents(int groupId)
        {
            return null;
        }

        public List<CommentInfo> GetComments(int eventId)
        {
            List<CommentInfo> comments = new List<CommentInfo>();

            var commentIDs = (from c in db.EventToCommentRelations
                              where c.EventId == eventId
                              select c.EventId);

            foreach (var id in commentIDs)
            {
                comments.Add(TableToEntity((from c in db.Comments where c.CommentTableID == id select c).Single()));
            }
            return comments;
        }
        #endregion
        #region RecordTables
        #region Exists
        public bool ExistsUser(int userId)
            {
                return (1 == (from u in db.Users where u.UserTableID == userId select u).Count());
            }
            public bool ExistsGroup(int groupId)
            {
                return (1 == (from g in db.Groups where g.GroupTableID == groupId select g).Count());
            }
            public bool ExistsEvent(int eventId)
            {
                return (1 == (from e in db.Events where e.EventTableID == eventId select e).Count());
            }
            public bool ExistsComment(int commentId)
            {
                return (1 == (from c in db.Comments where c.CommentTableID == commentId select c).Count());
            }
            #endregion
            #region Create
            public bool CreateUser(UserInfo user)
            {
                db.Users.Add(EntityToTable(user));
                db.SaveChanges();
                return true;
            }
            public bool CreateGroup(GroupInfo group)
            {
                db.Groups.Add(EntityToTable(group));
                db.SaveChanges();
                return true;
            }
            public bool CreateEvent(EventInfo newEvent)
            {
                db.Events.Add(EntityToTable(newEvent));
                db.SaveChanges();
                return true;
            }
            public bool CreateComment(CommentInfo comment)
            {
                db.Comments.Add(EntityToTable(comment));
                db.SaveChanges();
                return false;
            }
            #endregion
            #region Remove
            public bool RemoveUser(int userId)
            {
                var user = (from u in db.Users
                                where u.UserTableID == userId
                                select u).Single();
                user.Active = false;
                db.SaveChanges();
                return false;
            }
            public bool RemoveGroup(int groupId)
            {
                var group = (from g in db.Groups
                                where g.GroupTableID == groupId
                                select g).Single();
                group.Active = false;
                db.SaveChanges();
                return false;
            }
            public bool RemoveEvent(int eventId)
            {
                var theEvent = (from e in db.Events
                                where e.EventTableID == eventId
                                select e).Single();
                theEvent.Active = false;
                db.SaveChanges();
                return false;
            }
            public bool RemoveComment(int commentId)
            {
                var comment = (from c in db.Comments
                                where c.CommentTableID == commentId
                                select c).Single();
                comment.Active = false;
                db.SaveChanges();
                return false;
            }
            #endregion
            #region Get
            public UserInfo GetUser(int userId)
            {
                UserTable user = (from u in db.Users
                                  where u.UserTableID == userId
                                  select u).Single();
                return TableToEntity(user);
            }

            public UserInfo GetUser(string userName)
            {
                UserTable user = (from u in db.Users
                                  where u.UserName == userName
                                  select u).Single();
                return TableToEntity(user);
            }

            public GroupInfo GetGroup(int groupId)
            {
                GroupTable group = (from g in db.Groups
                                    where g.GroupTableID == groupId
                                    select g).Single();
                return TableToEntity(group);
            }

            public EventInfo GetEvent(int eventId)
            {
                EventTable theEvent = (from e in db.Events
                                       where e.EventTableID == eventId
                                       select e).Single();
                return TableToEntity(theEvent);
            }

            public CommentInfo GetComment(int commentId)
            {
                CommentTable comment = (from c in db.Comments
                                        where c.CommentTableID == commentId
                                        select c).Single();
                return TableToEntity(comment);
            }
            #endregion
            #region Set
            public bool SetUser(UserInfo user)
            {
                return true;
            }
            public bool SetGroup(GroupInfo group)
            {
                return true;
            }
            public bool SetEvent(EventInfo newEvent)
            {
                return true;
            }
            public bool SetComment(CommentInfo comment)
            {
                return false;
            }
            #endregion
        #endregion
        #region RelationTable
            #region Exists
            public bool ExistsUserToUserRelation(int senderId, int receiverId)
            {
                return false;
            }

            public bool ExistsGroupToUserRelation(int groupId, int userId)
            {
                return false;
            }

            public bool ExistsEventToUserRelation(int eventId, int userId)
            {
                return false;
            }

            public bool ExistsGroupToEventRelation(int groupId, int EventId)
            {
                return false;
            }

            public bool ExistsEventToCommentRelation(int eventId, int commentId)
            {
                return false;
            }
            #endregion
            #region Create
            public bool CreateUserToUserRelation(int senderId, int receiverId)
            {
                return false;
            }

            public bool CreateGroupToUserRelation(int groupId, int userId)
            {
                return false;
            }
            
            public bool CreateEventToUserRelation(int eventId, int userId)
            {
                return false;
            }

            public bool CreateGroupToEventRelation(int groupId, int EventId)
            {
                return false;
            }

            public bool CreateEventToCommentRelation(int eventId, int commentId)
            {
                return false;
            }
            #endregion
            #region Remove
            public bool RemoveUserToUserRelation(int id)
            {
                return false;
            }

            public bool RemoveGroupToUserRelation(int id)
            {
                return false;
            }

            public bool RemoveEventToUserRelation(int id)
            {
                return false;
            }

            public bool RemoveGroupToEventRelation(int id)
            {
                return false;
            }

            public bool RemoveEventToCommentRelation(int id)
            {
                return false;
            }
            #endregion
            #region Get
            public bool GetUserToUserRelation(int senderId, int receiverId)
            {
                return false;
            }

            public bool GetGroupToUserRelation(int groupId, int userId)
            {
                return false;
            }

            public bool GetEventToUserRelation(int eventId, int userId)
            {
                return false;
            }

            public bool GetGroupToEventRelation(int groupId, int EventId)
            {
                return false;
            }

            public bool GetEventToCommentRelation(int eventId, int commentId)
            {
                return false;
            }
            #endregion    
            #region Set
            public bool SetUserToUserRelation(int id, bool answer)
            {
                return false;
            }

            public bool SetEventToUserRelation(int id, bool answer)
            {
                return false;
            }
            #endregion
        #endregion
        #region TableToEntity
        protected UserInfo TableToEntity(UserTable table)
        {
            UserInfo info = new UserInfo();

            info.Id = table.UserTableID;
            info.UserName = table.UserName;
            info.DisplayName = table.DisplayName;
            info.Age = table.Age;
            info.Email = table.Email;
            info.Gender = table.Gender;

            return info;
        }
        protected GroupInfo TableToEntity(GroupTable table)
        {
            GroupInfo info = new GroupInfo();

            info.Id = table.GroupTableID;
            info.OwnerId = table.OwnerId;
            info.GroupName = table.Name;

            return info;
        }
        protected EventInfo TableToEntity(EventTable table)
        {
            EventInfo info = new EventInfo();

            info.Id = table.EventTableID;
            info.GroupId = table.GroupId;
            info.OwnerId = table.OwnerId;
            info.Name = table.Name;
            info.Photo = table.Photo;
            info.Description = table.Description;
            info.CreationTime = table.CreationTime;
            info.TimeOfEvent = table.TimeOfEvent;
            info.Minutes = table.Minutes;
            info.Location = table.Location;
            info.Answer = false;
            info.Max = table.Max;
            info.Min = table.Min;

            return info;
        }
        protected CommentInfo TableToEntity(CommentTable table)
        {
            CommentInfo info = new CommentInfo();

            info.Id = table.CommentTableID;
            info.OwnerId = table.OwnerId;
            info.Content = table.Content;
            info.CreationTime = table.CreationTime;

            return info;
        }
        #endregion
        #region EntityToTable

        protected UserTable EntityToTable(UserInfo user)
        {
            UserTable table = new UserTable();

            table.UserTableID = user.Id;
            table.Active = true;
            table.UserName = user.UserName;
            table.Gender = user.Gender;
            table.DisplayName = user.DisplayName;
            table.Age = user.Age;
            table.Email = user.Email;

            return table;
        }

        protected GroupTable EntityToTable(GroupInfo group)
        {
            GroupTable table = new GroupTable();

            table.GroupTableID = group.Id;
            table.Active = true;
            table.OwnerId = group.OwnerId;
            table.Name = group.GroupName;

            return table;
        }

        protected EventTable EntityToTable(EventInfo theEvent)
        {
            EventTable table = new EventTable();

            table.EventTableID = theEvent.Id;
            table.GroupId = theEvent.GroupId;
            table.OwnerId = theEvent.OwnerId;
            table.Name = theEvent.Name;
            table.Photo = theEvent.Photo;
            table.Description = theEvent.Description;
            table.CreationTime = theEvent.CreationTime;
            table.TimeOfEvent = theEvent.TimeOfEvent;
            table.Minutes = theEvent.Minutes;
            table.Location = theEvent.Location;
            table.Max = theEvent.Max;
            table.Min = theEvent.Min;

            return table;
        }

        protected CommentTable EntityToTable(CommentInfo comment)
        {
            CommentTable table = new CommentTable();

            table.CommentTableID = comment.Id;
            table.OwnerId = comment.OwnerId;
            table.Content = comment.Content;
            table.CreationTime = comment.CreationTime;

            return table;
        }

        #endregion
    }
}