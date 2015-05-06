using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace doStuff.Models.DatabaseModels
{
    public class EventToUserRelationTable
    {
        [Key]
        public int EventToUserRelationTableID { get; set; }
        public bool Active { get; set; }
        [ForeignKey("EventTable")]
        public uint EventId { get; set; }
        [ForeignKey("UserTable")]
        public uint AttendeeId { get; set; }
        public bool Answer { get; set; }
    }
}