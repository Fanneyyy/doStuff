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
        public string Owner { get; set; }
        public Event Event { get; set; }
        public bool? Attending { get; set; }
        public double TimeCreated { get; set; }
        public List<CommentViewModel> CommentsViewModels { get; set; }
        public List<User> Attendees { get; set; }
        public State State { get; set; }
    }
}