using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using doStuff.Models.DatabaseModels;

namespace doStuff.ViewModels
{
    public class SideBarViewModel
    {
        public User User { get; set; }
        public List<User> UserList { get; set; }
        public List<User> UserRequestList { get; set; }
        public List<EventViewModel> EventList { get; set; }
    }
}