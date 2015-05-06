using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using doStuff.POCOs;

namespace doStuff.Databases
{
    public class DatabaseUser : DatabaseGroup
    {
        public List<UserInfo> GetFriends(int userId)
        {
            return null;
        }

        public List<EventInfo> GetEvents(int userId)
        {
            return null;
        }

        public bool CreateFriendRequest(int senderId, int receiverId)
        {
            return false;
        }

        public bool AnswerFriendRequest(int senderId, int receiverId, bool answer)
        {
            return false;
        }

        public bool RemoveFriend(int senderId, int receiverId)
        {
            return false;
        }

        public bool ChangeName(int userId, string newName)
        {
            return false;
        }
    }
}