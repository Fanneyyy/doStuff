using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using doStuff.POCOs;

namespace doStuff.Databases
{
    public class DatabaseUser : DatabaseGroup
    {
        public List<UserInfo> GetFriends(uint userId)
        {
            return null;
        }

        public List<EventInfo> GetEvents(uint userId)
        {
            return null;
        }

        public bool CreateFriendRequest(uint senderId, uint receiverId)
        {
            return false;
        }

        public bool AnswerFriendRequest(uint senderId, uint receiverId, bool answer)
        {
            return false;
        }

        public bool RemoveFriend(uint senderId, uint receiverId)
        {
            return false;
        }

        public bool ChangeName(uint userId, string newName)
        {
            return false;
        }
    }
}