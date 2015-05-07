using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace doStuff.Exceptions
{
    public class GroupNotFoundException : Exception
    {

        public GroupNotFoundException()
            : base()
        {

        }

        public GroupNotFoundException(string message)
            : base(message)
        {

        }

        public GroupNotFoundException(string message, Exception inner)
            : base(message, inner)
        {

        }

    }
}