using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using doStuff.Models.DatabaseModels;

namespace doStuff.ViewModels
{
    public class SideBarViewModel
    {
        public User User;
        public List<User> UserList;
        public List<EventViewModel> EventList;
    }
}