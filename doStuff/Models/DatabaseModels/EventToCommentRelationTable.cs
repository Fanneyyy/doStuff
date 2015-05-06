﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace doStuff.Models.DatabaseModels
{
    public class EventToCommentRelationTable
    {
        [Key]
        public int EventToCommentRelationTableID { get; set; }
        public bool Active { get; set; }
        [ForeignKey("EventTable")]
        public int EventId { get; set; }
        [ForeignKey("CommentTable")]
        public int CommentId { get; set; }
    }
}