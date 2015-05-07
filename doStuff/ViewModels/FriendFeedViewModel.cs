using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using doStuff.POCOs;

namespace doStuff.ViewModels
{
    public class FriendFeedViewModel
    {
        SideBarViewModel SideBar { get; set; }
        List<UserInfo> RequestList { get; set; }
    }
}