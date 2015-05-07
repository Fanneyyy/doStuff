using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using doStuff.Models.DatabaseModels;

namespace doStuff.POCOs
{
    public class UserInfo
    {
        public int Id;
        public string UserName;
        public string DisplayName;
        public int Age;
        public Gender Gender;
        public string Email;
    }
}