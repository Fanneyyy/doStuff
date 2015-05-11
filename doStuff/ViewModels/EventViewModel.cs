using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using doStuff.Models.DatabaseModels;

namespace doStuff.ViewModels
{
    public enum State { ON = 0, OFF = 1, REACHED = 2, NOTREACHED = 3, FULL = 4 };

    public class EventViewModel
    {
        public string Owner;
        public Event Event;
        public bool? Attending;
        public double TimeCreated;
        public List<CommentViewModel> CommentsViewModels;
        public List<User> Attendees;
        public State State;
    }
}