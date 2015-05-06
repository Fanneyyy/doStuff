﻿using System;
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
        public int GroupToUserRelationTableID { get; set; }
        public bool Active { get; set; }
        [ForeignKey("GroupTable")]
        public int GroupId { get; set; }
        [ForeignKey("UserTable")]
        public int MemberId { get; set; }
    }
}