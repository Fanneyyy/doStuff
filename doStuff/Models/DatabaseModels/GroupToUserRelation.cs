using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace doStuff.Models.DatabaseModels
{
    public class GroupToUserRelation
    {
        [Key]
        public int GroupToUserRelationID { get; set; }
        public bool Active { get; set; }
        public int GroupId { get; set; }
        public int MemberId { get; set; }

        public GroupToUserRelation()
        {

        }
        public GroupToUserRelation(bool active, int groupId, int memberId)
        {
            Active = active;
            GroupId = groupId;
            MemberId = memberId;
        }
    }
}