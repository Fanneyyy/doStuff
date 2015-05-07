using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace doStuff.Models.DatabaseModels
{
    public class Comment
    {
        [Key]
        public int CommentID { get; set; }
        public bool Active { get; set; }
        public int OwnerId { get; set; }
        public string Content { get; set; }
        public DateTime CreationTime { get; set; }

        public Comment()
        {

        }
        public Comment(bool active, int ownerId, string content, DateTime creationTime)
        {
            Active = active;
            OwnerId = ownerId;
            Content = content;
            CreationTime = creationTime;
        }
    }
}