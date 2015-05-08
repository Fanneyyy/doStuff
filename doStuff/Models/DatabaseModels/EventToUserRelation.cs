using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace doStuff.Models.DatabaseModels
{
    public class EventToUserRelation
    {
        [Key]
        public int EventToUserRelationID { get; set; }
        public bool Active { get; set; }
        public int EventId { get; set; }
        public int AttendeeId { get; set; }
        public bool? Answer { get; set; }

        public EventToUserRelation()
        {

        }
        public EventToUserRelation(bool active, int eventId, int attendeeId, bool? answer, int id = 0)
        {
            EventToUserRelationID = id;
            Active = active;
            EventId = eventId;
            AttendeeId = attendeeId;
            Answer = answer;
        }
    }
}