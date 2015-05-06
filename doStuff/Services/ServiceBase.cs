using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using doStuff.POCOs;
using doStuff.ViewModels;
using doStuff.Databases;

namespace doStuff.Services
{
    public class ServiceBase
    {
        private static DatabaseBase db = new DatabaseBase();
        public bool CreateUser(UserInfo user)
        {
            return db.CreateUser(user);
        }

        public int GetUserId(string userName)
        {
            //TODO: throw exception
            UserInfo user = new UserInfo();
            user = db.GetUser(userName);

            return user.Id;
        }

        public bool IsOwnerOfEvent(int userId, int eventId)
        {
            EventInfo newEvent = getEventById(eventId);

            if (newEvent.OwnerId == userId)
            {
                return true;
            }
            return false;
        }

        public bool IsAttendingEvent(int userId, int eventId)
        {
            //TODO
            return false;
        }

        public bool IsOwnerOfComment(int userId, int commentId)
        {
            //TODO Exceptions if commentid and userid dont have anything attached
            CommentInfo newComment = getCommentById(commentId);

            if (newComment.OwnerId == userId)
            {
                return true;
            }
            return false;
        }

        public bool HasAccessToEvent(int userId, int eventId)
        {
            // TODO
            return false;
        }

        public bool RemoveEvent(int eventId)
        {
            return db.RemoveEvent(eventId);
        }

        public bool CreateComment(int eventId, CommentInfo comment)
        {
            //TODO
            return false;
        }

        public bool AnswerEvent(int userId, int eventId, bool answer)
        {
            return db.AnswerEvent(userId, eventId, answer);
        }

        private EventInfo getEventById(int eventId)
        {
            EventInfo newEvent = new EventInfo();
            newEvent = db.GetEvent(eventId);
            return newEvent;
        }
        private CommentInfo getCommentById(int commentId)
        {
            CommentInfo newComment = new CommentInfo();
            newComment = db.GetComment(commentId);
            return newComment;
        }
    }
}