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

        public static bool operator ==(EventToUserRelation relation1, EventToUserRelation relation2)
        {
            if (((object)relation1 == null) && ((object)relation2 == null))
            {
                return true;
            }
            if ((object)relation1 == null || (object)relation2 == null)
            {
                return false;
            }

            return (relation1.EventToUserRelationID == relation2.EventToUserRelationID)
                && (relation1.Active == relation2.Active)
                && (relation1.EventId == relation2.EventId)
                && (relation1.AttendeeId == relation2.AttendeeId)
                && (relation1.Answer == relation2.Answer);
        }

        public static bool operator !=(EventToUserRelation relation1, EventToUserRelation relation2)
        {
            return !(relation1 == relation2);
        }
    }
}