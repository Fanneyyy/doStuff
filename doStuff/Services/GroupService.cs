﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using doStuff.Models.DatabaseModels;
using doStuff.ViewModels;
using doStuff.Databases;

namespace doStuff.Services
{
    public class GroupService : ServiceBase
    {
        private static DatabaseGroup db = new DatabaseGroup(null);

        public EventFeedViewModel GetGroupFeed(uint groupId)
        {
            //TODO
            return null;
        }

        public bool IsOwnerOfGroup(uint UserId, uint groupId)
        {
            //TODO
            return false;
        }

        public bool IsMemberOfGroup(uint userId, uint groupId)
        {
            //TODO
            return false;
        }

        public bool AddMember(uint userId, uint groupId)
        {
            //TODO
            return false;
        }

        public bool RemoveMember(uint userId, uint groupId)
        {
            //TODO
            return false;
        }

        public bool CreateEvent(uint userId, uint groupId, Event newEvent)
        {
            //TODO
            return false;
        }

        public bool ChangeName(uint groupId, string newName)
        {
            //TODO
            return false;
        }

        public bool CreateGroup(Group group)
        {
            //TODO
            return false;
        }

        public bool RemoveGroup(uint groupId)
        {
            //TODO
            return false;
        }
    }
}