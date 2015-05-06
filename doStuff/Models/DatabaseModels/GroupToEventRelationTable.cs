﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace doStuff.Models.DatabaseModels
{
    public class GroupToEventRelationTable
    {
        [Key]
        public int GroupToEventRelationTableID { get; set; }
        public bool Active { get; set; }
        [ForeignKey("GroupId")]
        public int GroupId { get; set; }
        [ForeignKey("EventTable")]
        public int EventId { get; set; }
    }
}