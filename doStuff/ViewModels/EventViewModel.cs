using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using doStuff.Models.DatabaseModels;

namespace doStuff.ViewModels
{
    public class EventViewModel
    {
        public string Owner;
        public Event Event;
        public bool? Attending;
        public List<CommentViewModel> CommentsViewModels;
    }
}