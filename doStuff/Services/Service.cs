using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using doStuff.Models.DatabaseModels;
using doStuff.ViewModels;
using doStuff.Databases;
using doStuff.Exceptions;

namespace doStuff.Services
{
    public class Service
    {
        private static Database db = new Database();

        public EventFeedViewModel GetGroupFeed(int groupId, int userId)
        {
            //TODO
            // Show something if user has no friends or events?
            EventFeedViewModel feed = new EventFeedViewModel();
            List<EventViewModel> eventViews = new List<EventViewModel>();
            List<Event> events = db.GetEvents(groupId);

            foreach (Event eachEvent in events)
            {
                EventViewModel eventView = new EventViewModel();
                eventView.Owner = db.GetUser(eachEvent.OwnerId).UserName;
                eventView.Event = eachEvent;
                eventView.Comments = db.GetComments(eachEvent.EventID);
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
            List<Event> events = db.GetEvents(userId);

            foreach (Event eachEvent in events)
            {
                EventViewModel eventView = new EventViewModel();
                eventView.Owner = db.GetUser(eachEvent.OwnerId).UserName;
                eventView.Event = eachEvent;
                eventView.Comments = db.GetComments(eachEvent.EventID);
                eventViews.Add(eventView);
            }

            SideBarViewModel sidebar = new SideBarViewModel();
            sidebar.User = db.GetUser(userId);
            sidebar.UserList = db.GetFriends(userId);
            feed.Events = eventViews;
            feed.SideBar = sidebar;

            return feed;
        }

        public bool CreateUser(User user)
        {
            return db.CreateUser(user);
        }

        public int GetUserId(string userName)
        {
            //TODO: Throw exception

            User user = new User();
            user = db.GetUser(userName);

            return user.UserID;
        }

        public bool IsOwnerOfEvent(int userId, int eventId)
        {
            Event newEvent = GetEventById(eventId);

            if (newEvent.OwnerId == userId)
            {
                return true;
            }
            return false;
        }

        public bool IsFriendsWith(int userId, int friendId)
        {

            List<User> friends = db.GetFriends(userId);
            foreach (User a in friends)
            {
                if (a.UserID == friendId)
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
            //TODO finish this
            if (db.ExistsEventToUserRelation(eventId, userId)) 
            {
                int relationId = db.GetEventToUserRelation(eventId, userId);
                /*if (attending)
                {
                    return true;
                }
                else{
                    return false;
                }
            */
            }
            return false;
        }

        public bool IsOwnerOfComment(int userId, int commentId)
        {
            //TODO Exceptions if commentid and userid dont have anything attached
            Comment newComment = getCommentById(commentId);

            if (newComment.OwnerId == userId)
            {
                return true;
            }
            return false;
        }

        public bool CreateEvent(Event newEvent)
        {
            bool created = false;
            created = db.CreateEvent(newEvent);

            if (created)
            {
                return db.CreateEventToUserRelation(newEvent.EventID, newEvent.OwnerId);
            }
            return false;
        }

        public bool ChangeUserName(int userId, string newName)
        {
            if (db.ExistsUser(userId))
            {
                User user = db.GetUser(userId);
                user.UserName = newName;
                return db.SetUser(user);
            }
            //TODO: throw exeptions
            return false;
        }

        public bool HasAccessToEvent(int userId, int eventId)
        {
            Event thisEvent = db.GetEvent(eventId);
            if (db.ExistsUserToUserRelation(thisEvent.OwnerId, userId))
            {
                return true;
            }
            else if (thisEvent.GroupId.HasValue)
            {
                int groupId = (int)thisEvent.GroupId;
                if (db.ExistsGroupToUserRelation(groupId, userId)) 
                {
                    return true;
                }
            }
            return false;
        }

        public bool RemoveEvent(int eventId)
        {
            return db.RemoveEvent(eventId);
        }

        public bool CreateComment(int eventId, Comment comment)
        {
            //TODO Kasta villum ef event finnst ekki / virkar ekki
            db.CreateComment(comment);
            Event thisEvent = db.GetEvent(eventId);
            return db.CreateEventToCommentRelation(thisEvent.EventID, comment.CommentID);
        }

        public bool AnswerEvent(int userId, int eventId, bool answer)
        {
            if (db.ExistsEventToUserRelation(eventId, userId))
            {
                int relationId = db.GetEventToUserRelation(eventId, userId);
                return db.SetEventToUserRelation(relationId, answer);
            }
            
            return false;
        }

        // Group related service

        public bool IsOwnerOfGroup(int userId, int groupId)
        {
            Group group = db.GetGroup(groupId);

            if (userId == group.OwnerId)
            {
                return true;
            }

            return false;
        }

        public bool IsMemberOfGroup(int userId, int groupId)
        {
            List<User> groupMembers = db.GetMembers(groupId);

            foreach (User x in groupMembers)
            {
                if (x.UserID == userId)
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

        private Event getEventById(int eventId)
        {
            Event newEvent = new Event();
            newEvent = db.GetEvent(eventId);
            return newEvent;
        }

        private Comment getCommentById(int commentId)
        {
            Comment newComment = new Comment();
            newComment = db.GetComment(commentId);
            return newComment;
        }

        public bool ChangeGroupName(int groupId, string newName)
        {
            //TODO Exception ef grouId finnst ekki..

            Group group = db.GetGroup(groupId);
            group.Name = newName;
            return db.SetGroup(group);
        }
        public bool CreateGroup(Group group)
        {
            //TODO make user join group automatically
            bool created = false;

            created = db.CreateGroup(group);

            if (created)
            {
                return db.CreateGroupToUserRelation(group.GroupID, group.OwnerId);
            }
            return false;
        }

        public bool RemoveGroup(int groupId)
        {
            return db.RemoveGroup(groupId);
        }
        private Event GetEventById(int eventId)
        {
            Event newEvent = new Event();
            newEvent = db.GetEvent(eventId);
            return newEvent;
        }

        private Group getGroupById(int groupId)
        {
            Group newGroup = new Group();
            newGroup = db.GetGroup(groupId);
            return newGroup;
        }
    }
}