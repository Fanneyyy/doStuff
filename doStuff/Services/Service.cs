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
        private static Database db = null;
        public Service(Database database = null)
        {
            db = database ?? new Database(null); 
        }

        #region AccessRights
        public bool IsFriendsWith(int userId, int friendId)
        {
            List<User> friends = db.GetFriends(userId);

            foreach (User friend in friends)
            {
                if (friend.UserID == friendId)
                {
                    return true;
                }
            }

            return false;
        }
        public bool IsOwnerOfGroup(int userId, int groupId)
        {
            Group group = db.GetGroup(groupId);

            if (group == null)
            {
                return false;
            }
            if (userId == group.OwnerId)
            {
                return true;
            }

            return false;
        }
        public bool IsMemberOfGroup(int userId, int groupId)
        {
            List<User> members = db.GetMembers(groupId);

            foreach (User member in members)
            {
                if (member.UserID == userId)
                {
                    return true;
                }
            }

            return false;
        }
        public bool IsOwnerOfEvent(int userId, int eventId)
        {
            Event newEvent = GetEventById(eventId);

            if (newEvent != null && newEvent.OwnerId == userId)
            {
                return true;
            }
            return false;
        }
        public bool IsAttendingEvent(int userId, int eventId)
        {
            EventToUserRelation relation = db.GetEventToUserRelation(eventId, userId);
            if(relation == null || relation.Answer.HasValue == false)
            {
                return false;
            }
            return relation.Answer.Value;
        }
        public bool IsInvitedToEvent(int userId, int eventId)
        {
            Event theEvent = db.GetEvent(eventId);
            if(theEvent == null)
            {
                return false;
            }
            if(userId == theEvent.EventID)
            {
                return true;
            }
            if(theEvent.GroupId.HasValue)
            {
                return IsMemberOfGroup(userId, theEvent.GroupId.Value);
            }
            return IsFriendsWith(userId, theEvent.OwnerId);
        }
        public bool IsOwnerOfComment(int userId, int commentId)
        {
            Comment comment = GetCommentById(commentId);
            return (comment != null && userId == comment.OwnerId);
        }
        #endregion
        #region GetByID
        public int GetUserId(string userName)
        {
            User user = new User();
            user = db.GetUser(userName);
            if (user != null)
            {
                return user.UserID;
            }
            else
            {
                throw new UserNotFoundException();
            }
        }
        public Group GetGroupById(int groupId)
        {
            Group newGroup = new Group();
            newGroup = db.GetGroup(groupId);
            return newGroup;
        }
        public Event GetEventById(int eventId)
        {
            Event newEvent = new Event();
            newEvent = db.GetEvent(eventId);
            return newEvent;
        }
        public Comment GetCommentById(int commentId)
        {
            //TODO: Do Exception for Comment?
            Comment newComment = new Comment();
            newComment = db.GetComment(commentId);
            return newComment;
        }
        #endregion
        #region FriendRelations
        public bool AnswerFriendRequest(int userId, int senderId, bool answer)
        {
            UserToUserRelation relation = db.GetUserToUserRelation(senderId, userId);
            if(relation != null && relation.Active)
            {
                relation.Answer = answer;
                return db.SetUserToUserRelation(relation);
            }
            return false;
        }
        public bool RemoveFriend(int userId, int friendId)
        {
            if (IsFriendsWith(userId, friendId))
            {
                UserToUserRelation relation = db.GetUserToUserRelation(userId, friendId);
                if (relation == null || relation.Active == false)
                {
                    relation = db.GetUserToUserRelation(friendId, userId);
                }
                relation.Answer = false;
                return db.SetUserToUserRelation(relation);
            }
            //TODO REMOVE IF STATEMENT
            throw new Exception("You Tried To Remove A Friend Without Checking IsFriendsWith(userId, friendId) In The Controller First!!!");
        }
        public bool SendFriendRequest(int userId, int friendId)
        {
            if (!db.ExistsUserToUserRelation(userId, friendId))
            {
                UserToUserRelation relation = new UserToUserRelation();
                relation.Active = true;
                relation.SenderId = userId;
                relation.ReceiverId = friendId;
                relation.Answer = null;
                return db.CreateUserToUserRelation(ref relation);
            }
            return false;
        }
        #endregion
        #region GroupRelations
        public bool AddMember(int userId, int groupId)
        {
            
            if (!db.ExistsGroupToUserRelation(groupId, userId))
            {
                GroupToUserRelation relation = new GroupToUserRelation();
                relation.Active = true;
                relation.GroupId = groupId;
                relation.MemberId = userId;
                return db.CreateGroupToUserRelation(ref relation);
            }
            GroupToUserRelation existingRelation = db.GetGroupToUserRelation(groupId, userId);
            existingRelation.Active = true;
            return db.SetGroupToUserRelation(existingRelation);
        }
        public bool RemoveMember(int userId, int groupId)
        {
            GroupToUserRelation relation = db.GetGroupToUserRelation(groupId, userId);

            if (relation == null)
            {
                return false;
            }
            return db.RemoveGroupToUserRelation(relation.GroupToUserRelationID);
        }
        #endregion
        #region EventRelation
        public bool AnswerEvent(int userId, int eventId, bool answer)
        {
            if (IsInvitedToEvent(userId, eventId))
            {
                EventToUserRelation relation = db.GetEventToUserRelation(eventId, userId);
                if (relation == null)
                {
                    relation = new EventToUserRelation();
                    relation.EventId = eventId;
                    relation.AttendeeId = userId;
                    relation.Answer = answer;
                    return db.CreateEventToUserRelation(ref relation);
                }
                relation.Answer = answer;
                return db.SetEventToUserRelation(relation);
            }
            throw new Exception("You Tried To Answer An Event Without Checking IsInvitedToEvent(userId, eventId) In The Controller First!!!");
        }
        #endregion
        #region Create
        public bool CreateUser(User user)
        {
            return db.CreateUser(ref user);
        }
        public bool CreateGroup(Group group)
        {
            if (db.CreateGroup(ref group))
            {
                GroupToUserRelation relation = new GroupToUserRelation();
                relation.GroupId = group.GroupID;
                relation.MemberId = group.OwnerId;
                relation.Active = true;
                db.CreateGroupToUserRelation(ref relation);
                return true;
            }
            return false;
        }
        public bool CreateEvent(Event newEvent)
        {
            if(db.CreateEvent(ref newEvent))
            {
                EventToUserRelation relation = new EventToUserRelation();
                relation.EventId = newEvent.EventID;
                relation.AttendeeId = newEvent.OwnerId;
                relation.Active = true;
                relation.Answer = true;
                if(db.CreateEventToUserRelation(ref relation))
                {
                    if(newEvent.GroupId.HasValue)
                    {
                        GroupToEventRelation relation2 = new GroupToEventRelation();
                        relation2.EventId = newEvent.EventID;
                        relation2.GroupId = newEvent.GroupId.Value;
                        relation2.Active = true;
                        if(db.CreateGroupToEventRelation(ref relation2))
                        {
                            return true;
                        }
                        throw new Exception("The Event was created but an error occured when creation the GroupToUserRelation");
                    }
                    return true;
                }
                throw new Exception("The Event was created but an error occured when creation the EventToUserRelation");
            }
            return false;
        }
        public bool CreateComment(int eventId, Comment comment)
        {
            if (IsInvitedToEvent(comment.OwnerId, eventId))
            {
                if (db.CreateComment(ref comment))
                {
                    EventToCommentRelation relation = new EventToCommentRelation(true, eventId, comment.CommentID);
                    if (db.CreateEventToCommentRelation(ref relation))
                    {
                        return true;
                    }
                    throw new Exception("The comment was created but an error occured when creating the EventToCommentRelation");
                }
                return false;
            }
            throw new Exception("CreatedComment was called with out checking if IsInvitedToEvent in the controller first");
        }
        #endregion
        #region Remove
        public bool RemoveGroup(int groupId)
        {
            return db.RemoveGroup(groupId);
        }
        public bool RemoveEvent(int eventId)
        {
            return db.RemoveEvent(eventId);
        }
        public bool RemoveComment(int commentId)
        {
            return db.RemoveComment(commentId);
        }
        #endregion
        #region Edit
        public bool ChangeDisplayName(int userId, string newName)
        {
            if (db.ExistsUser(userId))
            {
                User user = db.GetUser(userId);
                user.DisplayName = newName;
                return db.SetUser(user);
            }
            throw new UserNotFoundException();
        }
        public bool ChangeGroupName(int groupId, string newName)
        {
            Group group = db.GetGroup(groupId);
            if (group == null)
            {
                throw new GroupNotFoundException();
            }
            group.Name = newName;
            return db.SetGroup(group);
        }
        #endregion
        #region GetViewModel
        public GroupFeedViewModel GetGroupFeed(int groupId, int userId)
        {
            //TODO
            // Show something if user has no friends or events?

            GroupFeedViewModel feed = new GroupFeedViewModel();
            List<EventViewModel> eventViews = new List<EventViewModel>();
            List<Event> events = db.GetGroupEvents(groupId);

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
            feed.groupId = groupId;


            List<Group> groups = db.GetGroups(userId);
            feed.Groups = groups;

            return feed;
        }
        public EventFeedViewModel GetEventFeed(int userId)
        {
            EventFeedViewModel eventFeed = new EventFeedViewModel();
            eventFeed.Events = new List<EventViewModel>();
            List<User> friends = db.GetFriends(userId);
            List<Event> events = new List<Event>();
            foreach(User friend in friends)
            {
                events = events.Concat(db.GetEvents(friend.UserID)).ToList();
            }
            List<Group> groups = db.GetGroups(userId);
            foreach (Group group in groups)
            {
                events = events.Concat(db.GetGroupEvents(group.GroupID)).ToList();
            }
            events = events.Concat(db.GetEvents(userId)).ToList();
            events.Sort(delegate(Event e1, Event e2) 
                        { 
                            //Sorting the list by when it was created
                            return e2.CreationTime.CompareTo(e1.CreationTime); 
                        });
            foreach(Event e in events)
            {
                EventViewModel eventViewModel = new EventViewModel();
                eventViewModel.Owner = db.GetUser(e.OwnerId).DisplayName;
                eventViewModel.Event = e;
                eventViewModel.Comments = db.GetComments(e.EventID);
                eventFeed.Events.Add(eventViewModel);
            }
            eventFeed.Groups = groups;
            eventFeed.SideBar = new SideBarViewModel();
            eventFeed.SideBar.User = db.GetUser(userId);
            eventFeed.SideBar.UserList = friends;
            return eventFeed;
        }
        #region SideBar
        private SideBarViewModel GetUserSideBar(int userId)
        {
            return null;
        }
        #endregion
        #endregion
    }
}