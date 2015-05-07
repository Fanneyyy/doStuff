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
    }
}