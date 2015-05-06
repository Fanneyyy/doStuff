using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace doStuff.Models.DatabaseModels
{
    public class EventTable
    {
        [Key]
        public int EventTableID { get; set; }
        public bool Active { get; set; }
        [ForeignKey("GroupTable")]
        public int GroupId { get; set; }
        [ForeignKey("UserTable")]
        public int OwnerId { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public string Description { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime TimeOfEvent { get; set; }
        public uint Minutes { get; set; }
        public string Location { get; set; }
        public uint Min { get; set; }
        public uint Max { get; set; }
    }
}