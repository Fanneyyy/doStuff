using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using doStuff.Models.DatabaseModels;

namespace doStuff.ViewModels
{
    public class CommentViewModel
    {
        public User Owner { get; set; }
        public Comment Comment { get; set; }
    }
}