using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace doStuff.Models.DatabaseModels
{
    public class Event
    {
        [Key]
        public int EventID { get; set; }
        public bool Active { get; set; }
        public int? GroupId { get; set; }
        public int OwnerId { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public string Description { get; set; }
        public DateTime CreationTime { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime TimeOfEvent { get; set; }
        public int Minutes { get; set; }
        public string Location { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }

        public Event()
        {

        }
        public Event(bool active, int? groupId, int ownerId, string name, string photo, string description, DateTime creationTime, DateTime timeOfEvent, int minutes, string location, int min, int max, int id = 0)
        {
            EventID = id;
            Active = active;
            GroupId = groupId;
            OwnerId = ownerId;
            Name = name;
            Photo = photo;
            Description = description;
            CreationTime = creationTime;
            TimeOfEvent = timeOfEvent;
            Minutes = minutes;
            Location = location;
            Min = min;
            Max = max;
        }

    }
}