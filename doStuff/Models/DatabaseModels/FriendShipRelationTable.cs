using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace doStuff.Models.DatabaseModels
{
    public class FriendShipRelationTable
    {
        [Key]
        public int EventToUserRelationTableID { get; set; }
        public bool Active { get; set; }
        [ForeignKey("UserTable")]
        public int SenderId { get; set; }
        [ForeignKey("UserTable")]
        public int ReceiverId { get; set; }
        public bool Answer { get; set; }
    }
}