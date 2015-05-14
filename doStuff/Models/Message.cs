using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace doStuff.Models
{
    public enum MessageType { ERROR, WARNING, INFORMATION, SUCCESS }
    public class Message
    {
        public MessageType Type { get; private set; }
        public string ErrorMessage { get; private set; }
        public string WarningMessage { get; private set; }
        public string InformationMessage { get; private set; }
        public string SuccessMessage { get; private set; }

        public Message()
        {

        }

        public Message(string message, MessageType type)
        {
            Type = type;
            switch (type)
            {
                case MessageType.ERROR:
                    ErrorMessage = message;
                    break;
                case MessageType.WARNING:
                    WarningMessage = message;
                    break;
                case MessageType.INFORMATION:
                    InformationMessage = message;
                    break;
                case MessageType.SUCCESS:
                    SuccessMessage = message;
                    break;
            }
        }
    }
}