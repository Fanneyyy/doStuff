using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using doStuff.Models.DatabaseModels;
using doStuff.ViewModels;

namespace doStuff.Services
{
    public class ServiceBase
    {
        public bool CreateUser(User user)
        {
            //TODO
            return false;
        }

        public uint GetUserId(string userName)
        {
            //TODO
            return 0;
        }

        public bool IsOwnerOfEvent(uint userId, uint eventId)
        {
            //TODO
            return false;
        }

        public bool IsAttendingEvent(uint userId, uint eventId)
        {
            //TODO
            return false;
        }

        public bool IsOwnerOfComment(uint userId, uint commentId)
        {
            //TODO
            return false;
        }

        public bool HasAccessToEvent(uint userId, uint eventId)
        {
            //TODO
            return false;
        }

        public bool RemoveEvent(uint eventId)
        {
            //TODO
            return false;
        }

        public bool CreateComment(uint eventId, Comment comment)
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