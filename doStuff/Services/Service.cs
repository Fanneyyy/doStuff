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

        public User GetUser(int id)
        {
            return db.GetUser(id);
        }

        public User GetUser(string userName)
        {
            return db.GetUser(userName);
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
            Group newGroup = db.GetGroup(groupId);
            return newGroup;
        }
        public Event GetEventById(int eventId)
        {
            Event newEvent = db.GetEvent(eventId);
            return newEvent;
        }
        public Comment GetCommentById(int commentId)
        {
            //TODO: Do Exception for Comment?
            Comment newComment = db.GetComment(commentId);
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
            //if user1 sends a friend request to user2 and user2 already sent a request, then they become friends
            if (db.ExistsUserToUserRelation(friendId, userId))
            {
                UserToUserRelation relation = db.GetUserToUserRelation(friendId, userId);
                if (relation.Answer == null)
                {
                    AnswerFriendRequest(userId, friendId, true);
                    return true;
                }
            }
            else if (!db.ExistsUserToUserRelation(userId, friendId))
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
                    relation.Active = true;
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
        public FriendProfileViewModel GetFriendFeed(int friendId)
        {
            FriendProfileViewModel profile = new FriendProfileViewModel();

            profile.Profile = db.GetUser(friendId);
            profile.Friends = db.GetFriends(friendId);
            List<Event> events = db.GetEvents(friendId);
            foreach(Event e in events)
            {
                profile.Events.Add(CastEventToViewModel(e));
            }

            return profile;
        }
        public EventFeedViewModel GetEventFeed(int userId)
        {
            EventFeedViewModel eventFeed = new EventFeedViewModel();
            eventFeed.Groups = db.GetGroups(userId);
            eventFeed.SideBar = GetSideBar(userId);
            List<Event> events = GetEventsFromFriends(eventFeed.SideBar.UserList);
            events = events.Concat(db.GetEvents(userId)).ToList();
            events = events.Concat(GetEventsFromGroups(eventFeed.Groups)).ToList();
            events = GetSortedEventList(events);
            eventFeed.Events = new List<EventViewModel>();
            foreach(Event e in events)
            {
                eventFeed.Events.Add(CastEventToViewModel(e));
            }
            return eventFeed;
        }
        #region ViewModelHelperFunctions
        private SideBarViewModel GetSideBar(int userId, int? groupId = null)
        {
            SideBarViewModel SideBar = new SideBarViewModel();

            SideBar.User = db.GetUser(userId);
            if (groupId.HasValue)
            {
                SideBar.UserList = db.GetMembers(groupId.Value);
                SideBar.UserList.Remove(SideBar.User);
            }
            else
            {
                SideBar.UserList = db.GetFriends(userId);
            }

            SideBar.UserList = GetSortedUserList(SideBar.UserList);

            return SideBar;
        }
        private EventViewModel CastEventToViewModel(Event e)
        {
            EventViewModel eventViewModel = new EventViewModel();
            eventViewModel.Owner = db.GetUser(e.OwnerId).DisplayName;
            eventViewModel.Event = e;
            eventViewModel.Comments = db.GetComments(e.EventID);
            return eventViewModel;
        }
        private List<Event> GetEventsFromFriends(List<User> friends)
        {
            List<Event> list = new List<Event>();
            foreach (User friend in friends)
            {
                list = list.Concat(db.GetEvents(friend.UserID)).ToList();
            }
            return list;
        }
        private List<Event> GetEventsFromGroups(List<Group> groups)
        {
            List<Event> list = new List<Event>();

            foreach (Group group in groups)
            {
                list = list.Concat(db.GetGroupEvents(group.GroupID)).ToList();
            }

            return list;
        }
        private List<Event> GetSortedEventList(List<Event> list)
        {
            list.Sort(delegate(Event e1, Event e2)
                      {
                          return e2.CreationTime.CompareTo(e1.CreationTime);
                      });
            return list;
        }
        private List<User> GetSortedUserList(List<User> list)
        {
            list.Sort(delegate(User u1, User u2)
                      {
                          return u1.DisplayName.CompareTo(u2.DisplayName);
                      });
            return list;
        }
        #endregion
        #endregion
    }
}