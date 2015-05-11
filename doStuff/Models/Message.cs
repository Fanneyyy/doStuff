using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace doStuff.Models
{
    public enum MessageType { Error, Warning, Information, Success}
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
            switch(type)
            {
                case MessageType.Error:
                    ErrorMessage = message;
                    break;
                case MessageType.Warning:
                    WarningMessage = message;
                    break;
                case MessageType.Information:
                    InformationMessage = message;
                    break;
                case MessageType.Success:
                    SuccessMessage = message;
                    break;
            }
        }
    }
}