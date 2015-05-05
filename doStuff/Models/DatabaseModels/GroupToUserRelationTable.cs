using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace doStuff.Models.DatabaseModels
{
    public class GroupToUserRelationTable
    {
        [Key]
        public uint Id { get; set; }
        public bool Active { get; set; }
        public uint GroupId { get; set; }
        public uint MemberId { get; set; }
    }
}