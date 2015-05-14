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
        [Required(ErrorMessage = "Please add a name to the event")]
        public string Name { get; set; }
        public string Photo { get; set; }
        [Required(ErrorMessage = "Please add a description to the event")]
        public string Description { get; set; }
        public DateTime CreationTime { get; set; }
        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "Please add a time to the event")]
        public DateTime TimeOfEvent { get; set; }
        [Required]
        [Range(13, 60, ErrorMessage="Minutes must be between 13 and 60")]
        public int Minutes { get; set; }
        [Required(ErrorMessage = "Please add a location to the event")]
        public string Location { get; set; }
        [Range(1, 100, ErrorMessage = "Min must be between 1 - 100")]
        public int? Min { get; set; }
        [Range(1, 100, ErrorMessage = "Max must be between 1 - 100")]
        public int? Max { get; set; }

        public Event()
        {

        }
        public Event(bool active, int? groupId, int ownerId, string name, string photo, string description, DateTime creationTime, DateTime timeOfEvent, int minutes, string location, int? min, int? max, int id = 0)
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