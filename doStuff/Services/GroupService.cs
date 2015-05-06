using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using doStuff.POCOs;
using doStuff.ViewModels;
using doStuff.Databases;

namespace doStuff.Services
{
    public class GroupService : ServiceBase
    {
        private static DatabaseGroup db = new DatabaseGroup();

        public EventFeedViewModel GetGroupFeed(int groupId)
        {
            //TODO
            // NEED BACKUP
            EventFeedViewModel feed = new EventFeedViewModel();
            
            return null;
        }

        public bool IsOwnerOfGroup(int UserId, int groupId)
        {
            //TODO
            GroupInfo group = db.GetGroup(groupId);

            if (groupId == group.OwnerId)
            {
                return true;
            }
            
            return false;
        }

        public bool IsMemberOfGroup(int userId, int groupId)
        {
            //TODO
            List <UserInfo> groupMembers = db.GetMembers(groupId);

            foreach (UserInfo x in groupMembers)
            {
                if (x.Id == userId) {
                    return true;
                }
            }
            return false;
        }

        public bool AddMember(int userId, int groupId)
        {
            return db.AddMember(groupId, userId);
        }

        public bool RemoveMember(int userId, int groupId)
        {
            return db.RemoveMember(groupId, userId);
        }

        public bool CreateEvent(int userId, int groupId, EventInfo newEvent)
        {
            // TODO: Add person automatically to event
            newEvent.GroupId = groupId;
            newEvent.OwnerId = userId;
            return db.CreateEvent(newEvent);
        }

        public bool ChangeName(int groupId, string newName)
        {
            //TODO Exception ef grouId finnst ekki..

            GroupInfo group = db.GetGroup(groupId);
            group.GroupName = newName;
            return true;
        }

        public bool CreateGroup(GroupInfo group)
        {
            //TODO make user join group automatically
            return db.CreateGroup(group);
        }

        public bool RemoveGroup(int groupId)
        {
            return db.RemoveGroup(groupId);
        }
        private EventInfo getEventById(int eventId)
        {
            EventInfo newEvent = new EventInfo();
            newEvent = db.GetEvent(eventId);
            return newEvent;
        }
        private GroupInfo getGroupById(int groupId)
        {
            GroupInfo newGroup = new GroupInfo();
            newGroup = db.GetGroup(groupId);
            return newGroup;
        }
        
    }
}