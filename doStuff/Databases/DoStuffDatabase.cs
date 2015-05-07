using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using doStuff.Models.DatabaseModels;
using System.Data.Entity.ModelConfiguration.Conventions;
using Microsoft.AspNet.Identity.EntityFramework;
using doStuff.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace doStuff.Databases
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    
    //ApplicationUser renamed to AppUser due to some conflict
    public class AppUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<AppUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public interface IDostuffDataContext
    {
        IDbSet<User> Users { get; set; }
        IDbSet<Group> Groups { get; set; }
        IDbSet<Event> Events { get; set; }
        IDbSet<Comment> Comments { get; set; }
        IDbSet<GroupToUserRelation> GroupToUserRelations { get; set; }
        IDbSet<UserToUserRelation> UserToUserRelations { get; set; }
        IDbSet<EventToUserRelation> EventToUserRelations { get; set; }
        IDbSet<EventToCommentRelation> EventToCommentRelations { get; set; }
        int SaveChanges();
    }
    public class DoStuffDatabase : IdentityDbContext<AppUser>, IDostuffDataContext
    {
        public IDbSet<User> Users { get; set; }
        public IDbSet<Group> Groups { get; set; }
        public IDbSet<Event> Events { get; set; }
        public IDbSet<Comment> Comments { get; set; }
        public IDbSet<GroupToUserRelation> GroupToUserRelations { get; set; }
        public IDbSet<UserToUserRelation> UserToUserRelations { get; set; }
        public IDbSet<EventToUserRelation> EventToUserRelations { get; set; }
        public IDbSet<EventToCommentRelation> EventToCommentRelations { get; set; }
        public DoStuffDatabase()
        : base("DefaultConnection")
        {
        }

        public static DoStuffDatabase Create()
        {
            return new DoStuffDatabase();
        }
    }
}