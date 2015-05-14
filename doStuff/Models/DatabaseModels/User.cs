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

        public static bool operator ==(User user1, User user2)
        {
            if (((object)user1 == null) && ((object)user2 == null))
            {
                return true;
            }
            if ((object)user1 == null || (object)user2 == null)
            {
                return false;
            }

            return (user1.UserID == user2.UserID)
                && (user1.Active == user2.Active)
                && (user1.UserName == user2.UserName)
                && (user1.DisplayName == user2.DisplayName)
                && (user1.BirthYear == user2.BirthYear)
                && (user1.Gender == user2.Gender)
                && (user1.Email == user2.Email);
        }
        public static bool operator !=(User user1, User user2)
        {
            return !(user1 == user2);
        }
    }
}