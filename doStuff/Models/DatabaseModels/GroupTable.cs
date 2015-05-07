using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace doStuff.Models.DatabaseModels
{
    public class GroupTable
    {
        [Key]
        public int GroupTableID { get; set; }
        public bool Active { get; set; }
        public int OwnerId { get; set; }
        public string Name { get; set; }
    }
}