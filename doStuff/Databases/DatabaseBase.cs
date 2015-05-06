﻿using System;
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

        #region GetInfo
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

        public List<GroupInfo> GetGroups(int userId)
        {
            List<GroupInfo> groups = new List<GroupInfo>();
            return groups;
        }

        public List<CommentInfo> GetComments(int eventId)
        {
            //TODO
            return null;
        }

        public bool CreateUser(UserInfo user)
        {
            //TODO
            return false;
        }

        public bool CreateEvent(EventInfo newEvent)
        {
            //TODO
            return false;
        }

        public bool RemoveEvent(int eventId)
        {
            //TODO
            return false;
        }

        public bool HasAccessToEvent(int userId, int eventId)
        {
            //TODO
            return false;
        }

        public bool CreateComment(int eventId, CommentInfo comment)
        {
            //TODO
            return false;
        }

        public bool RemoveComment(int commendId)
        {
            //TODO
            return false;
        }

        public bool AnswerEvent(int userId, int eventId, bool answer)
        {
            //TODO
            return false;
        }

        public bool IsAttendingEvent(int userId, int eventId)
        {
            return false;
        }

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
    }
}