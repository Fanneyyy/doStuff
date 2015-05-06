using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using doStuff.POCOs;
using doStuff.ViewModels;
using doStuff.Databases;

namespace doStuff.Services
{
    public class UserService : ServiceBase
    {
        private static DatabaseUser db = new DatabaseUser();

        public EventFeedViewModel GetEventFeed(uint userId)
        {
            //TODO
            return null;
        }

        public bool IsFriendsWith(uint userId, uint friendId)
        {
            //TODO
            return false;
        }

        public bool SendFriendRequest(uint userId, uint friendId)
        {
            //TODO
            return false;
        }

        public bool AnswerFriendRequest(uint userId, uint senderId, bool answer)
        {
            //TODO
            return false;
        }

        public bool RemoveFriend(uint userId, uint friendId)
        {
            //TODO
            return false;
        }

        public EventFeedViewModel GetFriendFeed(uint userId)
        {
            //TODO
            return null;
        }

        public bool CreateEvent(uint userId, EventInfo newEvent)
        {
            //TODO
            return false;
        }

        public bool ChangeName(uint userId, string newName)
        {
            //TODO
            return false;
        }
    }
}