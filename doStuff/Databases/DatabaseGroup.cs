using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using doStuff.POCOs;

namespace doStuff.Databases
{
    public class DatabaseGroup : DatabaseBase
    {
        public List<UserInfo> GetMembers(int groupId)
        {
            return null;
        }

        public List<EventInfo> GetEvents(int groupId)
        {
            return null;
        }

        public bool AddMember(int groupId, int memberId)
        {
            return false;
        }

        public bool RemoveMember(int groupId, int memberId)
        {
            return false;
        }

        public bool ChangeName(int groupId, string newName)
        {
            return false;
        }

        public bool CreateGroup(GroupInfo group)
        {
            return false;
        }

        public bool RemoveGroup(int groupId)
        {
            return false;
        }
    }
}