using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using doStuff.Models.DatabaseModels;

namespace doStuff.ViewModels
{
    public enum State { ON, OFF, REACHED, NOTREACHED, FULL };

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