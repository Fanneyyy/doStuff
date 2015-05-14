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
        public bool? Answer { get; set; }

        public UserToUserRelation()
        {

        }
        public UserToUserRelation(bool active, int senderId, int receiverId, bool? answer, int id = 0)
        {
            UserToUserRelationID = id;
            Active = active;
            SenderId = senderId;
            ReceiverId = receiverId;
            Answer = answer;
        }
        public static bool operator ==(UserToUserRelation relation1, UserToUserRelation relation2)
        {
            if (((object)relation1 == null) && ((object)relation2 == null))
            {
                return true;
            }
            if ((object)relation1 == null || (object)relation2 == null)
            {
                return false;
            }

            return (relation1.UserToUserRelationID == relation2.UserToUserRelationID)
                && (relation1.Active == relation2.Active)
                && (relation1.SenderId == relation2.SenderId)
                && (relation1.ReceiverId == relation2.ReceiverId)
                && (relation1.Answer == relation2.Answer)
                && (relation1.Active == relation2.Active);
        }

        public static bool operator !=(UserToUserRelation relation1, UserToUserRelation relation2)
        {
            return !(relation1 == relation2);
        }
    }
}