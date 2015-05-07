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
        public DateTime TimeOfEvent { get; set; }
        public int Minutes { get; set; }
        public string Location { get; set; }
        public uint Min { get; set; }
        public uint Max { get; set; }
    }
}