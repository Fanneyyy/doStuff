using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace doStuff.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException()
            : base()
        {

        }

        public UserNotFoundException(string message)
            : base(message)
        {

        }

        public UserNotFoundException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}