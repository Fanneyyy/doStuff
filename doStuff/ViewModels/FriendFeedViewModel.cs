using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using doStuff.Models.DatabaseModels;
namespace doStuff.ViewModels
{
    public class FriendFeedViewModel
    {
        SideBarViewModel SideBar { get; set; }
        List<User> RequestList { get; set; }
    }
}