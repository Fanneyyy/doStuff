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
        public string Name { get; set; }

        public Group()
        {

        }
        public Group(bool active, int ownerId, string name, int id)
        {
            GroupID = id;
            Active = active;
            OwnerId = ownerId;
            Name = name;
        }
    }
}