using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace doStuff.POCOs
{
    public class EventInfo
    {
        public int Id;
        public int GroupId;
        public int OwnerId;
        public string Name;
        public string Photo;
        public string Description;
        public DateTime CreationTime;
        public DateTime TimeOfEvent;
        public uint Minutes;
        public string Location;
        public bool Answer;
        public uint Max;
        public uint Min;
    }
}