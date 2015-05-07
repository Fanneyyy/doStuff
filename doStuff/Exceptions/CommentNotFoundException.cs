using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace doStuff.Exceptions
{
    public class CommentNotFoundException : Exception
    {

        public CommentNotFoundException()
            : base()
        {

        }

        public CommentNotFoundException(string message)
            : base(message)
        {

        }

        public CommentNotFoundException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}