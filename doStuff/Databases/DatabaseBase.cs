using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using doStuff.POCOs;

namespace doStuff.Databases
{
    public class DatabaseBase
    {
        public UserInfo GetUser(uint userId)
        {
            //TODO
            return null;
        }

        public UserInfo GetUser(string userName)
        {
            //TODO
            return null;
        }

        public GroupInfo GetGroup(uint groupId)
        {
            //TODO
            return null;
        }

        public EventInfo GetEvent(uint eventId)
        {
            //TODO
            return null;
        }

        public CommentInfo GetComment(uint commentId)
        {
            //TODO
            return null;
        }

        public List<GroupInfo> GetGroups(uint userId)
        {
            //TODO
            return null;
        }

        public List<CommentInfo> GetComments(uint eventId)
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

        public bool CreateComment(uint eventId, CommentInfo comment)
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