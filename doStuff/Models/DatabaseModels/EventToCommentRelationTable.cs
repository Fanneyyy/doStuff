using System;
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
        public uint Id { get; set; }
        public bool Active { get; set; }
        public uint EventId { get; set; }
        public uint CommentId { get; set; }
    }
}