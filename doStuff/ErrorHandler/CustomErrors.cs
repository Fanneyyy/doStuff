using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomErrors
{
    public enum LogType { None, TextFile, Email }

    public abstract class CustomException : Exception
    {
        public LogType LogType { get; set; }

        public CustomException(LogType Type = LogType.None)
            : base()
        {
            LogType = Type;
        }

        public CustomException(string message, LogType Type = LogType.None)
            : base(message)
        {
            LogType = Type;
        }

        public CustomException(string message, Exception inner, LogType Type = LogType.None)
            : base(message, inner)
        {
            LogType = Type;
        }
    }

    public class UserNotFoundException : CustomException
    {
        public UserNotFoundException(LogType Type = LogType.None)
            : base(Type)
        {

        }
        public UserNotFoundException(string message, LogType Type = LogType.None)
            : base(message, Type)
        {

        }
        public UserNotFoundException(string message, Exception inner, LogType Type = LogType.None)
            : base(message, inner, Type)
        {

        }
    }
    public class GroupNotFoundException : CustomException
    {

        public GroupNotFoundException(LogType Type = LogType.None)
            : base(Type)
        {

        }
        public GroupNotFoundException(string message, LogType Type = LogType.None)
            : base(message, Type)
        {

        }
        public GroupNotFoundException(string message, Exception inner, LogType Type = LogType.None)
            : base(message, inner, Type)
        {

        }

    }
    public class EventNotFoundException : CustomException
    {
        public EventNotFoundException(LogType Type = LogType.None)
            : base(Type)
        {

        }
        public EventNotFoundException(string message, LogType Type = LogType.None)
            : base(message, Type)
        {

        }
        public EventNotFoundException(string message, Exception inner, LogType Type = LogType.None)
            : base(message, inner, Type)
        {

        }
    }
    public class CommentNotFoundException : CustomException
    {
        public CommentNotFoundException(LogType Type = LogType.None)
            : base(Type)
        {

        }
        public CommentNotFoundException(string message, LogType Type = LogType.None)
            : base(message, Type)
        {

        }
        public CommentNotFoundException(string message, Exception inner, LogType Type = LogType.None)
            : base(message, inner, Type)
        {

        }
    }
}