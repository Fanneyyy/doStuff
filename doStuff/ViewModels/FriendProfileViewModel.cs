using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using doStuff.Models.DatabaseModels;

namespace doStuff.ViewModels
{
    public class FriendProfileViewModel
    {
        public User Profile { get; set; }
        public List<User> Friends { get; set; }
        public List<EventViewModel> Events { get; set; }
    }
}