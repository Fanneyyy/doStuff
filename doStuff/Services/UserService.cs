using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using doStuff.POCOs;
using doStuff.ViewModels;
using doStuff.Databases;

namespace doStuff.Services
{
    public class UserService : ServiceBase
    {
        private static DatabaseUser db = new DatabaseUser();

        public EventFeedViewModel GetEventFeed(int userId)
        {
            //TODO Show something if user has no friends or events?

            EventFeedViewModel feed = new EventFeedViewModel();
            List<EventViewModel> eventViews = new List<EventViewModel>();
            List<EventInfo> events = db.GetEvents(userId);

            foreach(EventInfo eachEvent in events)
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

        public bool IsFriendsWith(int userId, int friendId)
        {
          
            List<UserInfo> friends = db.GetFriends(userId);
            foreach(UserInfo a in friends) 
            {
                if (a.Id == friendId)
                {
                    return true;
                } 
            }

            return false;
        }

        public bool SendFriendRequest(int userId, int friendId)
        {
            //TODO Check if user has already sent a request before.

            return db.CreateFriendRequest(userId, friendId);
        }

        public bool AnswerFriendRequest(int userId, int senderId, bool answer)
        {
            //TODO Need function to check, if there is a friendrequest already
            if (answer == false)
            {
                return false;
            }

            return true;
   
        }

        public bool RemoveFriend(int userId, int friendId)
        {
      
            return db.RemoveFriend(userId, friendId);
        }

        public bool CreateEvent(int userId, EventInfo newEvent)
        {
            //TODO add user automaticly in event.

            return db.CreateEvent(newEvent);
        }

        public bool ChangeName(int userId, string newName)
        {
            return db.ChangeName(userId, newName);
        }
  
    }
}