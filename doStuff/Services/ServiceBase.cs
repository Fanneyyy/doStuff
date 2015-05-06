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

        public uint GetUserId(string userName)
        {
            //TODO: throw exception
            UserInfo user = new UserInfo();
            user = db.GetUser(userName);

            return user.Id;
        }

        public bool IsOwnerOfEvent(uint userId, uint eventId)
        {
            EventInfo newEvent = getEventById(eventId);

            if (newEvent.OwnerId == userId)
            {
                return true;
            }
            return false;
        }

        public bool IsAttendingEvent(uint userId, uint eventId)
        {
            //TODO
            // Need access to event users
            EventInfo newEvent = getEventById(eventId);

            return false;
        }

        public bool IsOwnerOfComment(uint userId, uint commentId)
        {
            //TODO Exceptions if commentid and userid dont have anything attached
            CommentInfo newComment = getCommentById(commentId);

            if (newComment.OwnerId == userId) ;
            {
                return true;
            }
            return false;
        }

        public bool HasAccessToEvent(uint userId, uint eventId)
        {
            //TODO
            // Need access to users of event
            return false;
        }

        public bool RemoveEvent(uint eventId)
        {
            return db.RemoveEvent(eventId);
        }

        public bool CreateComment(uint eventId, CommentInfo comment)
        {
            return db.CreateComment(eventId, comment);
        }

        public bool AnswerEvent(uint userId, uint eventId, bool answer)
        {
            return db.AnswerEvent(userId, eventId, answer);
        }

        private EventInfo getEventById(uint eventId)
        {
            EventInfo newEvent = new EventInfo();
            newEvent = db.GetEvent(eventId);
            return newEvent;
        }
        private CommentInfo getCommentById(uint commentId)
        {
            CommentInfo newComment = new CommentInfo();
            newComment = db.GetComment(commentId);
            return newComment;
        }
    }
}