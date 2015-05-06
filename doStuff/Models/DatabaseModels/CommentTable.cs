using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace doStuff.Models.DatabaseModels
{
    public class CommentTable
    {
        [Key]
        public int CommentTableID { get; set; }
        public bool Active { get; set; }
        [ForeignKey("UserTable")]
        public uint OwnerId { get; set; }
        public string Content { get; set; }
        public DateTime CreationTime { get; set; }
    }
}