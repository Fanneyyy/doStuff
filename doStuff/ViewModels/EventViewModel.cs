using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using doStuff.POCOs;

namespace doStuff.ViewModels
{
    public class EventViewModel
    {
        public string Owner;
        public EventInfo Event;
        public List<CommentInfo> Comments;
    }
}