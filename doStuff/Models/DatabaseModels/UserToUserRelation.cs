using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace doStuff.Models.DatabaseModels
{
    public class UserToUserRelation
    {
        [Key]
        public int UserToUserRelationID { get; set; }
        public bool Active { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public bool Answer { get; set; }
    }
}