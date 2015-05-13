using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using doStuff.Models.DatabaseModels;

namespace doStuff.ViewModels
{
    public class GroupFeedViewModel
    {
        public SideBarViewModel SideBar { get; set; }
        public List<EventViewModel> Events { get; set; }
        public List<Group> Groups { get; set; }
        public Group Group { get; set; }
    }
}