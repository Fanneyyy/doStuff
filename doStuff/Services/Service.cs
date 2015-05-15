using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using doStuff.Models.DatabaseModels;
using doStuff.ViewModels;
using doStuff.Databases;
using CustomErrors;

namespace doStuff.Services
{
    public class Service
    {
        private Database database;
        public Service(Database setDatabase = null)
        {
            database = setDatabase ?? new Database(null);
        }

        public User GetUser(int id)
        {
            return database.GetUser(id);
        }

        public User GetUser(string userName)
        {
            return database.GetUser(userName);
        }

        #region AccessRights
        public bool IsFriendsWith(int userId, int friendId)
        {
            UserToUserRelation r1 = database.GetUserToUserRelation(userId, friendId);
            UserToUserRelation r2 = database.GetUserToUserRelation(friendId, userId);

            if (r1 != null)
            {
                if (r1.Answer.HasValue && r1.Answer.Value)
                {
                    return true;
                }
            }
            if (r2 != null)
            {
                if (r2.Answer.HasValue && r2.Answer.Value)
                {
                    return true;
                }
            }
            return false;
        }
        public bool IsOwnerOfGroup(int userId, int groupId)
        {
            Group group = database.GetGroup(groupId);

            if (group == null)
            {
                return false;
            }
            return (userId == group.OwnerId);
        }
        public bool IsMemberOfGroup(int userId, int groupId)
        {
            var relation = database.GetGroupToUserRelation(groupId, userId);
            return (relation != null);
        }
        public bool IsEventInGroup(int groupId, int eventId)
        {
            var relation = database.GetGroupToEventRelation(groupId, eventId);
            return (relation != null);
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
            EventToUserRelation relation = database.GetEventToUserRelation(eventId, userId);
            if (relation == null || relation.Answer.HasValue == false)
            {
                return false;
            }
            return relation.Answer.Value;
        }
        public bool IsInvitedToEvent(int userId, int eventId)
        {
            Event theEvent = database.GetEvent(eventId);
            if (theEvent == null)
            {
                return false;
            }
            if (userId == theEvent.OwnerId)
            {
                return true;
            }
            if (theEvent.GroupId.HasValue)
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
        public Group GetGroupById(int groupId)
        {
            Group newGroup = database.GetGroup(groupId);
            return newGroup;
        }
        public Event GetEventById(int eventId)
        {
            Event newEvent = database.GetEvent(eventId);
            return newEvent;
        }
        public Comment GetCommentById(int commentId)
        {
            Comment newComment = database.GetComment(commentId);
            return newComment;
        }
        #endregion
        #region FriendRelations
        public bool AnswerFriendRequest(int senderId, int receiverId, bool answer)
        {
            UserToUserRelation relation = database.GetUserToUserRelation(senderId, receiverId);
            if (relation != null && relation.Active)
            {
                if (!answer)
                {
                    relation.Active = false;
                }
                if (relation.Answer != answer || relation.Active == false)
                {
                    relation.Answer = answer;
                    return database.Save();
                }
            }
            return false;
        }

        public bool RemoveFriend(int userId, int friendId)
        {
            UserToUserRelation relation = database.GetUserToUserRelation(userId, friendId);
            if (relation == null || relation.Active == false)
            {
                relation = database.GetUserToUserRelation(friendId, userId);
            }
            if (relation != null && relation.Active == true)
            {
                relation.Active = false;
                return database.Save();
            }
            return true;
        }

        public bool FriendRequestExists(int senderId, int receiverId)
        {
            return database.ExistsUserToUserRelation(senderId, receiverId);
        }

        public bool FriendRequestCancel(int senderId, int receiverId)
        {
            UserToUserRelation relation = database.GetUserToUserRelation(senderId, receiverId);
            if (relation != null && relation.Active == true)
            {
                relation.Active = false;
                return database.Save();
            }
            return true;
        }

        public bool SendFriendRequest(int userId, int friendId)
        {
            if (FriendRequestExists(friendId, userId))
            {
                return AnswerFriendRequest(friendId, userId, true);
            }
            UserToUserRelation relation = new UserToUserRelation();
            relation.Active = true;
            relation.SenderId = userId;
            relation.ReceiverId = friendId;
            relation.Answer = null;
            return database.CreateUserToUserRelation(ref relation);
        }
        #endregion
        #region GroupRelations
        public bool AddMember(int userId, int groupId)
        {
            GroupToUserRelation relation = new GroupToUserRelation();
            relation.Active = true;
            relation.GroupId = groupId;
            relation.MemberId = userId;
            return database.CreateGroupToUserRelation(ref relation);
        }
        public bool RemoveMember(int userId, int groupId)
        {
            GroupToUserRelation relation = database.GetGroupToUserRelation(groupId, userId);

            if (relation == null)
            {
                return false;
            }
            return database.RemoveGroupToUserRelation(relation.GroupToUserRelationID);
        }
        #endregion
        #region EventRelation
        public bool AnswerEvent(int userId, int eventId, bool answer)
        {
            EventToUserRelation relation = database.GetEventToUserRelation(eventId, userId);
            if (relation == null)
            {
                relation = new EventToUserRelation();
                relation.Active = true;
                relation.EventId = eventId;
                relation.AttendeeId = userId;
                relation.Answer = answer;
                return database.CreateEventToUserRelation(ref relation);
            }
            if (relation.Answer != answer)
            {
                relation.Answer = answer;
                return database.Save();
            }
            return true;
        }
        #endregion
        #region Create
        public bool CreateUser(ref User user)
        {
            return database.CreateUser(ref user);
        }
        public bool CreateGroup(ref Group group)
        {
            if (database.CreateGroup(ref group))
            {
                GroupToUserRelation relation = new GroupToUserRelation();
                relation.GroupId = group.GroupID;
                relation.MemberId = group.OwnerId;
                relation.Active = true;
                database.CreateGroupToUserRelation(ref relation);
                return true;
            }
            return false;
        }
        public bool CreateEvent(ref Event newEvent)
        {
            if (database.CreateEvent(ref newEvent))
            {
                EventToUserRelation relation = new EventToUserRelation();
                relation.EventId = newEvent.EventID;
                relation.AttendeeId = newEvent.OwnerId;
                relation.Active = true;
                relation.Answer = true;
                if (database.CreateEventToUserRelation(ref relation))
                {
                    if (newEvent.GroupId.HasValue)
                    {
                        GroupToEventRelation relation2 = new GroupToEventRelation();
                        relation2.EventId = newEvent.EventID;
                        relation2.GroupId = newEvent.GroupId.Value;
                        relation2.Active = true;
                        if (database.CreateGroupToEventRelation(ref relation2))
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
        public bool CreateComment(int eventId, ref Comment comment)
        {
            if (database.CreateComment(ref comment))
            {
                EventToCommentRelation relation = new EventToCommentRelation(true, eventId, comment.CommentID);
                if (database.CreateEventToCommentRelation(ref relation))
                {
                    return true;
                }
                throw new Exception("The comment was created but an error occured when creating the EventToCommentRelation");
            }
            return false;
        }
        #endregion
        #region GetViewModel
        public GroupFeedViewModel GetGroupFeed(int groupId, int userId)
        {
            GroupFeedViewModel groupFeed = new GroupFeedViewModel();
            groupFeed.Group = database.GetGroup(groupId);
            groupFeed.Groups = database.GetGroups(userId);
            groupFeed.SideBar = GetSideBar(userId, groupId);
            List<Event> events = database.GetGroupEvents(groupId);
            events = GetSortedEventList(events);
            groupFeed.Events = new List<EventViewModel>();
            foreach (Event e in events)
            {
                bool? attending;
                EventToUserRelation eventToUser = database.GetEventToUserRelation(e.EventID, userId);
                if (eventToUser == null)
                {
                    attending = null;
                }
                else
                {
                    attending = eventToUser.Answer;
                }
                // Checks if to add this to eventFeed
                EventViewModel temp = CastToViewModel(e, attending);
                // Adds all events to feed if user is attending or if the event has not expired.
                if (temp.Attending != true && (temp.State == State.OFF || temp.State == State.FULL || temp.State == State.ON))
                {
                }
                else
                {
                    groupFeed.Events.Add(temp);
                }
            }
            return groupFeed;
        }
        public EventFeedViewModel GetEventFeed(int userId)
        {
            EventFeedViewModel eventFeed = new EventFeedViewModel();
            eventFeed.Groups = database.GetGroups(userId);
            eventFeed.SideBar = GetSideBar(userId);
            List<Event> events = GetEventsFromFriends(eventFeed.SideBar.UserList);
            events = events.Concat(database.GetEvents(userId)).ToList();
            events = events.Concat(GetEventsFromGroups(eventFeed.Groups)).ToList();
            events = GetSortedEventList(events);
            eventFeed.Events = new List<EventViewModel>();
            foreach (Event e in events)
            {
                bool? attending;
                EventToUserRelation eventToUser = database.GetEventToUserRelation(e.EventID, userId);
                if (eventToUser == null)
                {
                    attending = null;
                }
                else
                {
                    attending = eventToUser.Answer;
                }
                // Checks if to add this to eventFeed
                EventViewModel temp = CastToViewModel(e, attending);
                // Adds all events to feed if user is attending or if the event has not expired.
                if (temp.Attending != true && (temp.State == State.OFF || temp.State == State.FULL || temp.State == State.ON))
                {
                }
                else
                {
                    eventFeed.Events.Add(temp);
                }

            }
            return eventFeed;
        }
        #region ViewModelHelperFunctions
        public SideBarViewModel GetSideBar(int userId, int? groupId = null)
        {
            SideBarViewModel SideBar = new SideBarViewModel();
            SideBar.Avatar = "~/Content/pictures/Avatars/avatar0" + (userId % 9 + 1) + ".jpg";
            if (userId == 1337) 
            {
                SideBar.Avatar = "~/Content/pictures/Avatars/surprise.jpg";
            }
            SideBar.User = database.GetUser(userId);
            SideBar.EventList = new List<EventViewModel>();


            if (groupId.HasValue)
            {
                SideBar.UserList = database.GetMembers(groupId.Value);
                SideBar.UserList.Remove(SideBar.User);
            }
            else
            {
                SideBar.UserList = GetSortedUserList(database.GetFriends(userId));
                SideBar.UserPendingList = GetSortedUserList(database.GetPendingRequests(userId));
                SideBar.UserRequestList = GetSortedUserList(database.GetFriendRequests(userId));
            }

            SideBar.UserList = GetSortedUserList(SideBar.UserList);
            List<Event> events = new List<Event>();
            if (groupId.HasValue)
            {
                events = database.GetGroupEvents((int)groupId);
            }
            else
            {
                events = GetEventsFromFriends(SideBar.UserList);
                events = events.Concat(database.GetEvents(userId)).ToList();
                events = events.Concat(GetEventsFromGroups(database.GetGroups(userId))).ToList();
                events = GetSortedByDateOfEvent(events);
            }



            foreach (Event e in events)
            {
                bool? attending;
                EventToUserRelation eventToUser = database.GetEventToUserRelation(e.EventID, userId);
                if (eventToUser == null)
                {
                    attending = null;
                }
                else
                {
                    attending = eventToUser.Answer;
                }
                // Converts the event e to EventViewModel.
                EventViewModel temp = CastToViewModel(e, attending);
                // Adds all events to feed if user is attending or if the event has not expired.
                if ((temp.Attending == true && (temp.State == State.ON)) && temp.Event.TimeOfEvent >= DateTime.Now)
                {
                    SideBar.EventList.Add(temp);
                }
            }


            return SideBar;
        }
        public EventViewModel CastToViewModel(Event e, bool? attending)
        {
            EventViewModel eventViewModel = new EventViewModel();
            List<CommentViewModel> commentViews = new List<CommentViewModel>();
            eventViewModel.Owner = database.GetUser(e.OwnerId).DisplayName;
            eventViewModel.Event = e;
            eventViewModel.Attending = attending;
            eventViewModel.Attendees = database.GetAttendees(e.EventID);
            List<Comment> comments = database.GetComments(e.EventID);
            foreach (Comment comment in comments)
            {
                commentViews.Add(CastToViewModel(comment));
            }
            eventViewModel.CommentsViewModels = commentViews;
            if (TimeLeft(e, DateTime.Now).TotalSeconds <= 0)
            {
                eventViewModel.State = State.OFF;
                if (!e.Min.HasValue || e.Min <= eventViewModel.Attendees.Count)
                {
                    eventViewModel.State = State.ON;
                }
            }
            else
            {
                eventViewModel.State = State.NOTREACHED;
                if (e.Max.HasValue && e.Max == eventViewModel.Attendees.Count)
                {
                    eventViewModel.State = State.FULL;
                }
                else if (!e.Min.HasValue || e.Min <= eventViewModel.Attendees.Count)
                {
                    eventViewModel.State = State.REACHED;
                }
            }
            eventViewModel.TimeCreated = DateTimeToMillis(e.CreationTime);
            return eventViewModel;
        }

        private CommentViewModel CastToViewModel(Comment c)
        {
            CommentViewModel commentViewModel = new CommentViewModel();
            commentViewModel.Comment = c;
            commentViewModel.Owner = database.GetUser(c.OwnerId);
            return commentViewModel;
        }

        private List<Event> GetEventsFromFriends(List<User> friends)
        {
            List<Event> list = new List<Event>();
            foreach (User friend in friends)
            {
                list = list.Concat(database.GetEvents(friend.UserID)).ToList();
            }
            return list;
        }
        private List<Event> GetEventsFromGroups(List<Group> groups)
        {
            List<Event> list = new List<Event>();

            foreach (Group group in groups)
            {
                list = list.Concat(database.GetGroupEvents(group.GroupID)).ToList();
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
        public List<Event> GetSortedByDateOfEvent(List<Event> list)
        {
            list.Sort(delegate(Event e1, Event e2)
            {
                return e1.TimeOfEvent.CompareTo(e2.TimeOfEvent);
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
        public static double DateTimeToMillis(DateTime created)
        {
            return created
               .Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc))
               .TotalMilliseconds;
        }

        public TimeSpan TimeLeft(Event time, DateTime now)
        {
            TimeSpan check = new TimeSpan(0, 0, 0);
            TimeSpan addMinutes = new TimeSpan(0, 0, time.Minutes, 0, 0);
            DateTime timeOfDecision = time.CreationTime;
            timeOfDecision = timeOfDecision.Add(addMinutes);
            TimeSpan timeToDecision = timeOfDecision.Subtract(now);

            if (timeToDecision <= check)
            {
                return check;
            }
            return timeToDecision;

        }

        public bool ValidationOfTimeOfEvent(Event thisEvent)
        {
            DateTime time = DateTime.Now;
            TimeSpan minutes = new TimeSpan(0, thisEvent.Minutes, 0);
            time = time.Add(minutes);
            return thisEvent.TimeOfEvent >= time;
        }
    }
}