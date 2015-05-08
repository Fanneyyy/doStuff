using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using doStuff.Models.DatabaseModels;

namespace doStuff.ViewModels
{
    public class GroupFeedViewModel
    {
        public SideBarViewModel SideBar;
        public List<EventViewModel> Events;
        public List<Group> Groups;
        public int groupId;
    }
}