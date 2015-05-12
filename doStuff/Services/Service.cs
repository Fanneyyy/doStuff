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
        private static Database db;
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

        #region AccessRights
        public bool IsFriendsWith(int userId, int friendId)
        {
            UserToUserRelation r1 = db.GetUserToUserRelation(userId, friendId);
            UserToUserRelation r2 = db.GetUserToUserRelation(friendId, userId);

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
        public bool IsEventInGroup(int groupId, int eventId)
        {
            List<Event> events = db.GetEvents(groupId);
            foreach (Event eventt in events)
            {
                if (eventId == eventt.EventID)
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
            if (relation == null || relation.Answer.HasValue == false)
            {
                return false;
            }
            return relation.Answer.Value;
        }
        public bool IsInvitedToEvent(int userId, int eventId)
        {
            Event theEvent = db.GetEvent(eventId);
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
        public bool AnswerFriendRequest(int senderId, int receiverId, bool answer)
        {
            UserToUserRelation relation = db.GetUserToUserRelation(senderId, receiverId);
            if (relation != null && relation.Active)
            {
                if (!answer)
                {
                    relation.Active = false;
                }
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
                relation.Active = false;
                return db.SetUserToUserRelation(relation);
            }
            //TODO REMOVE IF STATEMENT
            throw new Exception("You Tried To Remove A Friend Without Checking IsFriendsWith(userId, friendId) In The Controller First!!!");
        }

        public bool FriendRequestExists(int senderId, int receiverId)
        {
            return db.ExistsUserToUserRelation(senderId, receiverId);
        }

        public bool SendFriendRequest(int userId, int friendId)
        {
            UserToUserRelation relation = new UserToUserRelation();
            relation.Active = true;
            relation.SenderId = userId;
            relation.ReceiverId = friendId;
            relation.Answer = null;
            return db.CreateUserToUserRelation(ref relation);       
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
        public bool CreateUser(ref User user)
        {
            return db.CreateUser(ref user);
        }
        public bool CreateGroup(ref Group group)
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
        public bool CreateEvent(ref Event newEvent)
        {
            if (db.CreateEvent(ref newEvent))
            {
                EventToUserRelation relation = new EventToUserRelation();
                relation.EventId = newEvent.EventID;
                relation.AttendeeId = newEvent.OwnerId;
                relation.Active = true;
                relation.Answer = true;
                if (db.CreateEventToUserRelation(ref relation))
                {
                    if (newEvent.GroupId.HasValue)
                    {
                        GroupToEventRelation relation2 = new GroupToEventRelation();
                        relation2.EventId = newEvent.EventID;
                        relation2.GroupId = newEvent.GroupId.Value;
                        relation2.Active = true;
                        if (db.CreateGroupToEventRelation(ref relation2))
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
            GroupFeedViewModel groupFeed = new GroupFeedViewModel();
            groupFeed.Group = db.GetGroup(groupId);
            groupFeed.Groups = db.GetGroups(userId);
            groupFeed.SideBar = GetSideBar(userId, groupId);
            List<Event> events = db.GetGroupEvents(groupId);
            events = GetSortedEventList(events);
            groupFeed.Events = new List<EventViewModel>();
            foreach (Event e in events)
            {
                bool? attending;
                EventToUserRelation eventToUser = db.GetEventToUserRelation(e.EventID, userId);
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
                if (!(temp.Attending != true && (temp.State != State.REACHED ||temp.State == State.NOTREACHED)))
                {
                    groupFeed.Events.Add(temp);
                }
            }
            return groupFeed;
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
            foreach (Event e in events)
            {
                bool? attending;
                EventToUserRelation eventToUser = db.GetEventToUserRelation(e.EventID, userId);
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
                if (!(temp.Attending != true && (temp.State != State.REACHED || temp.State == State.NOTREACHED)))
                {
                    eventFeed.Events.Add(temp);
                }

            }
            return eventFeed;
        }
        #region ViewModelHelperFunctions
        private SideBarViewModel GetSideBar(int userId, int? groupId = null)
        {
            SideBarViewModel SideBar = new SideBarViewModel();

            SideBar.User = db.GetUser(userId);
            SideBar.EventList = new List<EventViewModel>();

            
            if (groupId.HasValue)
            {
                SideBar.UserList = db.GetMembers(groupId.Value);
                SideBar.UserList.Remove(SideBar.User);
            }
            else
            {
                SideBar.UserList = db.GetFriends(userId);
                SideBar.UserRequestList = db.GetFriendRequests(userId);
            }

            SideBar.UserList = GetSortedUserList(SideBar.UserList);
            List<Event> events = new List<Event>();
            if (groupId.HasValue)
            {
                events = db.GetGroupEvents((int)groupId);
            }
            else 
            {
                events = GetEventsFromFriends(SideBar.UserList);
                events = events.Concat(db.GetEvents(userId)).ToList();
                events = events.Concat(GetEventsFromGroups(db.GetGroups(userId))).ToList();
                events = GetSortedByDateOfEvent(events);
            }
            
            

            foreach (Event e in events)
            {
                bool? attending;
                EventToUserRelation eventToUser = db.GetEventToUserRelation(e.EventID, userId);
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
                if ((temp.Attending == true && (temp.State == State.ON )) && temp.Event.TimeOfEvent >= DateTime.Now)
                {
                    SideBar.EventList.Add(temp);
                }
            }


            return SideBar;
        }
        private EventViewModel CastToViewModel(Event e, bool? attending)
        {
            EventViewModel eventViewModel = new EventViewModel();
            List<CommentViewModel> commentViews = new List<CommentViewModel>();
            eventViewModel.Owner = db.GetUser(e.OwnerId).DisplayName;
            eventViewModel.Event = e;
            eventViewModel.Attending = attending;
            eventViewModel.Attendees = db.GetAttendees(e.EventID);
            List<Comment> comments = db.GetComments(e.EventID);
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
            commentViewModel.Owner = db.GetUser(c.OwnerId);
            return commentViewModel;
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
    }
}