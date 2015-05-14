using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace doStuff.Models.DatabaseModels
{
    public class GroupToEventRelation
    {
        [Key]
        public int GroupToEventRelationID { get; set; }
        public bool Active { get; set; }
        public int GroupId { get; set; }
        public int EventId { get; set; }

        public GroupToEventRelation()
        {

        }
        public GroupToEventRelation(bool active, int groupId, int eventId, int id = 0)
        {
            GroupToEventRelationID = id;
            Active = active;
            GroupId = groupId;
            EventId = eventId;
        }

        public static bool operator ==(GroupToEventRelation relation1, GroupToEventRelation relation2)
        {
            if (((object)relation1 == null) && ((object)relation2 == null))
            {
                return true;
            }
            if ((object)relation1 == null || (object)relation2 == null)
            {
                return false;
            }

            return (relation1.GroupToEventRelationID == relation2.GroupToEventRelationID)
                && (relation1.Active == relation2.Active)
                && (relation1.GroupId == relation2.GroupId)
                && (relation1.EventId == relation2.EventId);
        }

        public static bool operator !=(GroupToEventRelation relation1, GroupToEventRelation relation2)
        {
            return !(relation1 == relation2);
        }
    }
}