using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using doStuff.Models.DatabaseModels;
using doStuff.ViewModels;
using doStuff.Databases;
using ErrorHandler;

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

            if (events == null)
            {
                throw new EventNotFoundException();
            }

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


            List<Group> groups = db.GetGroups(userId);
            feed.Groups = groups;

            return feed;
        }

        public EventFeedViewModel GetEventFeed(int userId)
        {
            //TODO Show something if user has no friends or events?
            // Throw Event Exception.
            EventFeedViewModel feed = new EventFeedViewModel();
            List<EventViewModel> eventViews = new List<EventViewModel>();
            List<Event> events = db.GetEvents(userId);

            if (events == null)
            {
                throw new EventNotFoundException();
            }

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

            List<Group> groups = db.GetGroups(userId);
            feed.Groups = groups;

            return feed;
        }

        public bool CreateUser(User user)
        {
            return db.CreateUser(user);
        }

        public int GetUserId(string userName)
        {
            User user = new User();
            user = db.GetUser(userName);
            if (user == null)
            {
                throw new UserNotFoundException();
            }

            return user.UserID;
        }

        public bool IsOwnerOfEvent(int userId, int eventId)
        {
    
            Event newEvent = GetEventById(eventId);
            if (newEvent == null)
            {
                throw new EventNotFoundException();
            }
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

            if (db.ExistsUserToUserRelation(senderId, userId) || (db.ExistsUserToUserRelation(userId, senderId)))
            {
                UserToUserRelation relation = db.GetUserToUserRelation(senderId, userId);
                relation.Answer = answer;
                return db.SetUserToUserRelation(relation);
            }
            else
            {
                return false;
            }

        }

        public bool RemoveFriend(int userId, int friendId)
        {
            
            if (!db.ExistsUserToUserRelation(userId, friendId) && (!db.ExistsUserToUserRelation(friendId, userId)))
            {
                //TODO
            }
            UserToUserRelation relation = db.GetUserToUserRelation(userId, friendId);

            return false;
        }

        public bool SendFriendRequest(int userId, int friendId)
        {
            
            //TODO Check if user has already sent a request before.
            if (!db.ExistsUserToUserRelation(userId, friendId))
            {
                return false; // db.CreateUserToUserRelation(userId, friendId);
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
                EventToUserRelation relation = db.GetEventToUserRelation(eventId, userId);
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
            
            Comment newComment = getCommentById(commentId);

            if (newComment == null)
            {
                throw new CommentNotFoundException();
            }

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
                return false; // db.CreateEventToUserRelation(newEvent.EventID, newEvent.OwnerId);
            }
            return false;
        }

        public bool ChangeUserName(int userId, string newName)
        {
            //TODO: Throw User Exception.
            if (db.ExistsUser(userId))
            {
                User user = db.GetUser(userId);
                user.UserName = newName;
                return db.SetUser(user);
            }
            
            return false;
        }

        public bool HasAccessToEvent(int userId, int eventId)
        {
            //TODO: Throw Event Exception.
            Event thisEvent = db.GetEvent(eventId);

            if (thisEvent == null)
            {
                throw new EventNotFoundException();
            }
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
            //TODO: Throw Event Exception.

            db.CreateComment(comment);
            Event thisEvent = db.GetEvent(eventId);
            if (thisEvent == null)
            {
                throw new EventNotFoundException();
            }
            return false;
        }

        public bool AnswerEvent(int userId, int eventId, bool answer)
        {
            //TODO: Throw Event Exception.
            if (db.ExistsEventToUserRelation(eventId, userId))
            {
                EventToUserRelation relation = db.GetEventToUserRelation(eventId, userId);
                if (relation == null)
                {
                    throw new EventNotFoundException();
                }

                relation.Answer = answer;
                return db.SetEventToUserRelation(relation);
            }
            
            return false;
        }

        // Group related service

        public bool IsOwnerOfGroup(int userId, int groupId)
        {
            //TODO: Throw Group Exception.
            Group group = db.GetGroup(groupId);

            if (group == null)
            {
                throw new GroupNotFoundException();
            }
            if (userId == group.OwnerId)
            {
                return true;
            }

            return false;
        }

        public bool IsMemberOfGroup(int userId, int groupId)
        {
            //TODO: Throw Group Exception.
            List<User> groupMembers = db.GetMembers(groupId);

            if (groupMembers == null)
            {
                throw new GroupNotFoundException();
            }
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
            return false;// db.CreateGroupToUserRelation(groupId, userId);
        }

        public bool RemoveMember(int userId, int groupId)
        {
            //TODO: Throw User Exception.
            GroupToUserRelation relation = db.GetGroupToUserRelation(groupId, userId);

            if (relation == null)
            {
                throw new UserNotFoundException();
            }
            return db.RemoveGroupToUserRelation(relation.GroupToUserRelationID);
        }

        private Event getEventById(int eventId)
        {
            // TODO: Throw Event Exception.
            Event newEvent = new Event();

            if (newEvent == null)
            {
                throw new EventNotFoundException();
            }
            newEvent = db.GetEvent(eventId);
            return newEvent;
        }

        private Comment getCommentById(int commentId)
        {
            //TODO: Do Exception for Comment?
            Comment newComment = new Comment();

            if (newComment == null)
            {
                //throw new CommentNotFoundException();
            }
            newComment = db.GetComment(commentId);
            return newComment;
        }

        public bool ChangeGroupName(int groupId, string newName)
        {
            //TODO Throw Group Exception.

            Group group = db.GetGroup(groupId);

            if (group == null)
            {
                throw new GroupNotFoundException();
            }
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
                return false;// db.CreateGroupToUserRelation(group.GroupID, group.OwnerId);
            }
            return false;
        }

        public bool RemoveGroup(int groupId)
        {
            return db.RemoveGroup(groupId);
        }
        private Event GetEventById(int eventId)
        {
            //TODO: Throw Event Exception.
            Event newEvent = new Event();
            newEvent = db.GetEvent(eventId);

            if (newEvent == null)
            {
                throw new EventNotFoundException();
            }
            return newEvent;
        }

        private Group GetGroupById(int groupId)
        {
            //TODO: Throw Group Exception.
            Group newGroup = new Group();
            newGroup = db.GetGroup(groupId);

            if (newGroup == null)
            {
                throw new GroupNotFoundException();
            }
            return newGroup;
        }
    }
}