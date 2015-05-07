using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace doStuff.Exceptions
{
    public class FriendNotFoundException : Exception
    {

        public FriendNotFoundException()
            : base()
        {

        }

        public FriendNotFoundException(string message)
            : base(message)
        {

        }

        public FriendNotFoundException(string message, Exception inner)
            : base(message, inner)
        {

        }

    }
}