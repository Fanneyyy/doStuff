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
        public GroupToUserRelation(bool active, int groupId, int memberId, int id = 0)
        {
            GroupToUserRelationID = id;
            Active = active;
            GroupId = groupId;
            MemberId = memberId;
        }

        public static bool operator ==(GroupToUserRelation relation1, GroupToUserRelation relation2)
        {
            if (((object)relation1 == null) && ((object)relation2 == null))
            {
                return true;
            }
            if ((object)relation1 == null || (object)relation2 == null)
            {
                return false;
            }

            return (relation1.GroupToUserRelationID == relation2.GroupToUserRelationID)
                && (relation1.Active == relation2.Active)
                && (relation1.GroupId == relation2.GroupId)
                && (relation1.MemberId == relation2.MemberId);
        }

        public static bool operator !=(GroupToUserRelation relation1, GroupToUserRelation relation2)
        {
            return !(relation1 == relation2);
        }
    }
}