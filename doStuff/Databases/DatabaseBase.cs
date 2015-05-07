using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using doStuff.Models.DatabaseModels;

namespace doStuff.Databases
{
    public class DatabaseBase
    {
        private readonly IDostuffDataContext db;

        public DatabaseBase(IDostuffDataContext dbContext)
        {
            db = dbContext ?? new DoStuffDatabase();       
        }

        public User GetUser(int userId)
        {
            User user = (from u in db.Users
                             where u.UserID == userId
                             select u).First();
            return user;
        }

        public User GetUser(string userName)
        {
            //TODO
            return null;
        }

        public Group GetGroup(uint groupId)
        {
            //TODO
            return null;
        }

        public Event GetEvent(uint eventId)
        {
            //TODO
            return null;
        }

        public Comment GetComment(uint commentId)
        {
            //TODO
            return null;
        }

        public List<Group> GetGroups(uint userId)
        {
            //TODO
            return null;
        }

        public List<Comment> GetComments(uint eventId)
        {
            //TODO
            return null;
        }

        public bool CreateUser(User user)
        {

            db.Users.Add(user);
            db.SaveChanges();
            return false;
        }

        public bool CreateEvent(Event newEvent)
        {
            //TODO
            return false;
        }

        public bool RemoveEvent(uint eventId)
        {
            //TODO
            return false;
        }

        public bool HasAccessToEvent(uint userId, uint eventId)
        {
            //TODO
            return false;
        }

        public bool CreateComment(uint eventId, Comment comment)
        {
            //TODO
            return false;
        }

        public bool RemoveComment(uint commendId)
        {
            //TODO
            return false;
        }

        public bool AnswerEvent(uint userId, uint eventId, bool answer)
        {
            //TODO
            return false;
        }
    }
}