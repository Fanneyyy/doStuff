using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace doStuff.Models.DatabaseModels
{
    public enum Gender { MALE, FEMALE };

    public class User
    {
        [Key]
        public int UserID { get; set; }
        public bool Active { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public int BirthYear { get; set; }
        public Gender Gender { get; set; }
        public string Email { get; set; }

        public User()
        {

        }
        public User(bool active, string userName, string displayName, int birthYear, Gender gender, string email, int id = 0)
        {
            UserID = id;
            Active = active;
            UserName = userName;
            DisplayName = displayName;
            BirthYear = birthYear;
            Gender = gender;
            Email = email;
        }
    }
}