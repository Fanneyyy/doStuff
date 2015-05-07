using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using doStuff.POCOs;
using doStuff.ViewModels;
using doStuff.Databases;
using doStuff.Exceptions;

namespace doStuff.Services
{
    public class ServiceBase
    {
        private static Database db = new Database();

        public EventFeedViewModel GetGroupFeed(int groupId, int userId)
        {
            //TODO
            // Show something if user has no friends or events?
            EventFeedViewModel feed = new EventFeedViewModel();
            List<EventViewModel> eventViews = new List<EventViewModel>();
            List<EventInfo> events = db.GetEvents(groupId);

            foreach (EventInfo eachEvent in events)
            {
                EventViewModel eventView = new EventViewModel();
                eventView.Owner = db.GetUser(eachEvent.OwnerId).UserName;
                eventView.Event = eachEvent;
                eventView.Comments = db.GetComments(eachEvent.Id);
                eventViews.Add(eventView);
            }

            SideBarViewModel sidebar = new SideBarViewModel();
            sidebar.User = db.GetUser(userId);
            sidebar.UserList = db.GetMembers(groupId);
            feed.Events = eventViews;
            feed.SideBar = sidebar;


            return feed;
        }

        public EventFeedViewModel GetEventFeed(int userId)
        {
            //TODO Show something if user has no friends or events?

            EventFeedViewModel feed = new EventFeedViewModel();
            List<EventViewModel> eventViews = new List<EventViewModel>();
            List<EventInfo> events = db.GetEvents(userId);

            foreach (EventInfo eachEvent in events)
            {
                EventViewModel eventView = new EventViewModel();
                eventView.Owner = db.GetUser(eachEvent.OwnerId).UserName;
                eventView.Event = eachEvent;
                eventView.Comments = db.GetComments(eachEvent.Id);
                eventViews.Add(eventView);
            }

            SideBarViewModel sidebar = new SideBarViewModel();
            sidebar.User = db.GetUser(userId);
            sidebar.UserList = db.GetFriends(userId);
            feed.Events = eventViews;
            feed.SideBar = sidebar;

            return feed;
        }

        public bool CreateUser(UserInfo user)
        {
            return db.CreateUser(user);
        }

        public int GetUserId(string userName)
        {
            //TODO: Throw exception

            UserInfo user = new UserInfo();
            user = db.GetUser(userName);

            return user.Id;
        }

        public bool IsOwnerOfEvent(int userId, int eventId)
        {
            EventInfo newEvent = GetEventById(eventId);

            if (newEvent.OwnerId == userId)
            {
                return true;
            }
            return false;
        }

        public bool IsFriendsWith(int userId, int friendId)
        {

            List<UserInfo> friends = db.GetFriends(userId);
            foreach (UserInfo a in friends)
            {
                if (a.Id == friendId)
                {
                    return true;
                }
            }

            return false;
        }

        public bool AnswerFriendRequest(int userId, int senderId, bool answer)
        {
            if (db.ExistsUserToUserRelation(senderId, userId))
            {
                int relationId = db.GetUserToUserRelation(senderId, userId);
                return db.SetUserToUserRelation(relationId, answer);
            }
            else
            {
                return false;
            }

        }

        public bool RemoveFriend(int userId, int friendId)
        {
            int relationId = db.GetUserToUserRelation(userId, friendId);
            return db.RemoveUserToUserRelation(relationId);
        }

        public bool SendFriendRequest(int userId, int friendId)
        {
            //TODO Check if user has already sent a request before.
            if (!db.ExistsUserToUserRelation(userId, friendId))
            {
                return db.CreateUserToUserRelation(userId, friendId);
            }
            else
            {
                return false;
            }

        }

        public bool IsAttendingEvent(int userId, int eventId)
        {
            //TODO
            return false;
        }

        public bool IsOwnerOfComment(int userId, int commentId)
        {
            //TODO Exceptions if commentid and userid dont have anything attached
            CommentInfo newComment = getCommentById(commentId);

            if (newComment.OwnerId == userId)
            {
                return true;
            }
            return false;
        }

        public bool CreateEvent(EventInfo newEvent)
        {
            bool created = false;
            created = db.CreateEvent(newEvent);

            if (created)
            {
                return db.CreateEventToUserRelation(newEvent.Id, newEvent.OwnerId);
            }
            return false;
        }

        public bool ChangeUserName(int userId, string newName)
        {
            if (db.ExistsUser(userId))
            {
                UserInfo user = db.GetUser(userId);
                user.UserName = newName;
                return db.SetUser(user);
            }
            //TODO: throw exeptions
            return false;
        }

        public bool HasAccessToEvent(int userId, int eventId)
        {
            // TODO
            return false;
        }

        public bool RemoveEvent(int eventId)
        {
            return db.RemoveEvent(eventId);
        }

        public bool CreateComment(int eventId, CommentInfo comment)
        {
            //TODO
            return false;
        }

        public bool AnswerEvent(int userId, int eventId, bool answer)
        {
            return false;
        }

        // Group related service

        public bool IsOwnerOfGroup(int UserId, int groupId)
        {
            GroupInfo group = db.GetGroup(groupId);

            if (groupId == group.OwnerId)
            {
                return true;
            }

            return false;
        }

        public bool IsMemberOfGroup(int userId, int groupId)
        {
            List<UserInfo> groupMembers = db.GetMembers(groupId);

            foreach (UserInfo x in groupMembers)
            {
                if (x.Id == userId)
                {
                    return true;
                }
            }

            return false;
        }

        public bool AddMember(int userId, int groupId)
        {
            return db.CreateGroupToUserRelation(groupId, userId);
        }

        public bool RemoveMember(int userId, int groupId)
        {
            int relationId = db.GetGroupToUserRelation(groupId, userId);

            return db.RemoveGroupToUserRelation(relationId);
        }

        private EventInfo getEventById(int eventId)
        {
            EventInfo newEvent = new EventInfo();
            newEvent = db.GetEvent(eventId);
            return newEvent;
        }

        private CommentInfo getCommentById(int commentId)
        {
            CommentInfo newComment = new CommentInfo();
            newComment = db.GetComment(commentId);
            return newComment;
        }

        public bool ChangeGroupName(int groupId, string newName)
        {
            //TODO Exception ef grouId finnst ekki..

            GroupInfo group = db.GetGroup(groupId);
            group.GroupName = newName;
            return db.SetGroup(group);
        }
        public bool CreateGroup(GroupInfo group)
        {
            //TODO make user join group automatically
            bool created = false;

            created = db.CreateGroup(group);

            if (created)
            {
                return db.CreateGroupToUserRelation(group.Id, group.OwnerId);
            }
            return false;
        }

        public bool RemoveGroup(int groupId)
        {
            return db.RemoveGroup(groupId);
        }
        private EventInfo GetEventById(int eventId)
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