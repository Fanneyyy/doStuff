using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace doStuff.Models.DatabaseModels
{
    public class Group
    {
        [Key]
        public int GroupID { get; set; }
        public bool Active { get; set; }
        public int OwnerId { get; set; }
        [Required]
        public string Name { get; set; }

        public Group()
        {

        }
        public Group(bool active, int ownerId, string name, int id = 0)
        {
            GroupID = id;
            Active = active;
            OwnerId = ownerId;
            Name = name;
        }

        public static bool operator ==(Group group1, Group group2)
        {
            if (((object)group1 == null) && ((object)group2 == null))
            {
                return true;
            }
            if ((object)group1 == null || (object)group2 == null)
            {
                return false;
            }

            return (group1.GroupID == group2.GroupID)
                && (group1.Active == group2.Active)
                && (group1.OwnerId == group2.OwnerId)
                && (group1.Name == group2.Name);
        }

        public static bool operator !=(Group group1, Group group2)
        {
            return !(group1 == group2);
        }
    }
}