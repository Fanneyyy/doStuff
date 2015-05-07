using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace doStuff.Exceptions
{
    public class EventNotFoundException :Exception
    {

        public EventNotFoundException()
            : base()
        {

        }

        public EventNotFoundException(string message)
            : base(message)
        {

        }

        public EventNotFoundException(string message, Exception inner)
            : base(message, inner)
        {

        }

    }
}