using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace doStuff.Models.DatabaseModels
{
    public class EventToCommentRelation
    {
        [Key]
        public int EventToCommentRelationID { get; set; }
        public bool Active { get; set; }
        public int EventId { get; set; }
        public int CommentId { get; set; }

        public EventToCommentRelation()
        {

        }
        public EventToCommentRelation(bool active, int eventId, int commentId, int id = 0)
        {
            EventToCommentRelationID = id;
            Active = active;
            EventId = eventId;
            CommentId = commentId;
        }

        public static bool operator ==(EventToCommentRelation relation1, EventToCommentRelation relation2)
        {
            if (((object)relation1 == null) && ((object)relation2 == null))
            {
                return true;
            }
            if ((object)relation1 == null || (object)relation2 == null)
            {
                return false;
            }

            return (relation1.EventToCommentRelationID == relation2.EventToCommentRelationID)
                && (relation1.Active == relation2.Active)
                && (relation1.EventId == relation2.EventId)
                && (relation1.CommentId == relation2.CommentId);
        }

        public static bool operator !=(EventToCommentRelation relation1, EventToCommentRelation relation2)
        {
            return !(relation1 == relation2);
        }
    }
}