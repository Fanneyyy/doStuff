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

        public EventFeedViewModel GetEventFeed(int userId)
        {
            //TODO

            List<EventInfo> eventfeed = db.GetEvents(userId);

            return null;
        }

        public bool IsFriendsWith(int userId, int friendId)
        {
            //TODO
            List<UserInfo> friends = db.GetFriends(userId);
            foreach(UserInfo a in friends) 
            {
                if (a.Id == friendId)
                {
                    return true;
                } 
            }

            return false;
        }

        public bool SendFriendRequest(int userId, int friendId)
        {
            //TODO

           

            return false;
        }

        public bool AnswerFriendRequest(int userId, int senderId, bool answer)
        {
            //TODO
            return false;
        }

        public bool RemoveFriend(int userId, int friendId)
        {
            //TODO
            return false;
        }

        public EventFeedViewModel GetFriendFeed(int userId)
        {
            //TODO
            return null;
        }

        public bool CreateEvent(int userId, EventInfo newEvent)
        {
            //TODO
            return false;
        }

        public bool ChangeName(int userId, string newName)
        {
            //TODO
            return false;
        }
    }
}