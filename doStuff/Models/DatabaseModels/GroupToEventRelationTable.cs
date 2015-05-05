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
        public uint Id { get; set; }
        public bool Active { get; set; }
        [ForeignKey("GroupId")]
        public uint GroupId { get; set; }
        [ForeignKey("EventTable")]
        public uint EventId { get; set; }
    }
}