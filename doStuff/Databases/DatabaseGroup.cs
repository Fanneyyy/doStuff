using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using doStuff.POCOs;

namespace doStuff.Databases
{
    public class DatabaseGroup : DatabaseBase
    {
        public List<UserInfo> GetMembers(uint groupId)
        {
            return null;
        }

        public List<EventInfo> GetEvents(uint groupId)
        {
            return null;
        }

        public bool AddMember(uint groupId, uint memberId)
        {
            return false;
        }

        public bool RemoveMember(uint groupId, uint memberId)
        {
            return false;
        }

        public bool ChangeName(uint groupId, string newName)
        {
            return false;
        }

        public bool CreateGroup(GroupInfo group)
        {
            return false;
        }

        public bool RemoveGroup(uint groupId)
        {
            return false;
        }
    }
}