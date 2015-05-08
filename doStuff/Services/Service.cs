﻿using System;
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
        private static Database db = new Database(null);

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

            if (newEvent.OwnerId == userId)
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

            if(theEvent.GroupId.HasValue)
            {
                return IsMemberOfGroup(userId, theEvent.GroupId.Value);
            }
            return IsFriendsWith(userId, theEvent.OwnerId);
        }
        public bool IsOwnerOfComment(int userId, int commentId)
        {

            Comment comment = GetCommentById(commentId);
            return (userId == comment.OwnerId);
        }
        #endregion
        #region GetByID
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
        public Group GetGroupById(int groupId)
        {
            Group newGroup = new Group();
            newGroup = db.GetGroup(groupId);

            if (newGroup == null)
            {
                throw new GroupNotFoundException();
            }

            return newGroup;
        }
        public Event GetEventById(int eventId)
        {
            Event newEvent = new Event();
            newEvent = db.GetEvent(eventId);

            if (newEvent == null)
            {
                throw new EventNotFoundException();
            }
            return newEvent;
        }
        public Comment GetCommentById(int commentId)
        {
            //TODO: Do Exception for Comment?
            Comment newComment = new Comment();
            newComment = db.GetComment(commentId);

            if (newComment == null)
            {
                throw new CommentNotFoundException();
            }

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
            throw Exception("You Tried To Remove A Friend Without Checking IsFriendsWith(userId, friendId) In The Controller First!!!");
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
                return db.CreateUserToUserRelation(relation);
            }
            else
            {
                return false;
            }
        }
        #endregion
        #region GroupRelations
        public bool AddMember(int userId, int groupId)
        {
            if (IsOwnerOfGroup(userId, groupId))
            {
                GroupToUserRelation relation = db.GetGroupToUserRelation(groupId, userId);
                if (relation == null)
                {
                    relation = new GroupToUserRelation();
                    relation.Active = true;
                    relation.GroupId = groupId;
                    relation.MemberId = userId;
                    return db.CreateGroupToUserRelation(relation);
                }
                relation.Active = true;
                return db.SetGroupToUserRelation(relation);
            }
            throw Exception("You Tried To Add A Member Without Checking IsOwnerOfGroup(userId, groupId) In The Controller First!!!");
        }
        public bool RemoveMember(int userId, int groupId)
        {
            if (IsOwnerOfGroup(userId, groupId))
            {
                GroupToUserRelation relation = db.GetGroupToUserRelation(groupId, userId);

                if (relation == null)
                {
                    return false;
                }
                return db.RemoveGroupToUserRelation(relation.GroupToUserRelationID);
            }
            throw Exception("You Tried To Remove A Member Without Checking IsOwnerOfGroup(userId, groupId) In The Controller First!!!");
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
                    return db.CreateEventToUserRelation(relation);
                }
                relation.Answer = answer;
                return db.SetEventToUserRelation(relation);
            }
            throw Exception("You Tried To Answer An Event Without Checking IsInvitedToEvent(userId, eventId) In The Controller First!!!");
        }
        #endregion
        #region Create
        public bool CreateUser(User user)
        {
            return db.CreateUser(user);
        }
        public bool CreateGroup(Group group)
        {
            //TODO
            if (db.CreateGroup(group))
            {
                GroupToUserRelation relation = new GroupToUserRelation();
                relation.GroupId = group.GroupID;
                relation.MemberId = group.OwnerId;
                relation.Active = true;
                db.CreateGroupToUserRelation(relation);
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
                // Creating relation & attending for owner
                EventToUserRelation relation = new EventToUserRelation();
                relation.EventId = newEvent.EventID;
                relation.AttendeeId = newEvent.OwnerId;
                relation.Active = true;
                relation.Answer = true;
                db.CreateEventToUserRelation(relation);

                List <User> users = db.GetFriends(newEvent.OwnerId);

                foreach (User n in users)
                {
                    EventToUserRelation relationForFriend = new EventToUserRelation();
                    relationForFriend.EventId = newEvent.EventID;
                    relationForFriend.AttendeeId = n.UserID;
                    relationForFriend.Active = true;
                    db.CreateEventToUserRelation(relationForFriend);
                }
                return true;
            }
            return false;
        }
        public bool CreateComment(int eventId, Comment comment)
        {
            //TODO: Throw Event Exception.

            db.CreateComment(comment);
            Event thisEvent = GetEventById(eventId);

            return false;
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
            //TODO: Throw User Exception.
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
            feed.groupId = groupId;


            List<Group> groups = db.GetGroups(userId);
            feed.Groups = groups;

            return feed;
        }
        public EventFeedViewModel GetEventFeed(int userId)
        {
            //TODO Show something if user has no friends or events?
            // Throw Event Exception.
            // Muna að laga svo maður fái líka event frá friends
            EventFeedViewModel feed = new EventFeedViewModel();
            List<EventViewModel> eventViews = new List<EventViewModel>();
            List<Event> events = db.GetAllEventUserRelation(userId);

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
        #endregion
    }
}