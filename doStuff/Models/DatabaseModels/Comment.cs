﻿using System;
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
        [Required(ErrorMessage = "This field can't be empty.")]
        public string Content { get; set; }
        public DateTime CreationTime { get; set; }

        public Comment()
        {

        }
        public Comment(bool active, int ownerId, string content, DateTime creationTime, int id = 0)
        {
            CommentID = id;
            Active = active;
            OwnerId = ownerId;
            Content = content;
            CreationTime = creationTime;
        }
    }
}