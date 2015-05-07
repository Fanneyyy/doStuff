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
        IDbSet<UserTable> Users { get; set; }
        IDbSet<GroupTable> Groups { get; set; }
        IDbSet<EventTable> Events { get; set; }
        IDbSet<CommentTable> Comments { get; set; }
        IDbSet<GroupToUserRelationTable> GroupToUserRelations { get; set; }
        IDbSet<FriendShipRelationTable> FriendShipRelations { get; set; }
        IDbSet<EventToUserRelationTable> EventToUserRelations { get; set; }
        IDbSet<EventToCommentRelationTable> EventToCommentRelations { get; set; }
        int SaveChanges();
    }
    public class DoStuffDatabase : IdentityDbContext<AppUser>, IDostuffDataContext
    {
        public IDbSet<UserTable> Users { get; set; }
        public IDbSet<GroupTable> Groups { get; set; }
        public IDbSet<EventTable> Events { get; set; }
        public IDbSet<CommentTable> Comments { get; set; }
        public IDbSet<GroupToUserRelationTable> GroupToUserRelations { get; set; }
        public IDbSet<FriendShipRelationTable> FriendShipRelations { get; set; }
        public IDbSet<EventToUserRelationTable> EventToUserRelations { get; set; }
        public IDbSet<EventToCommentRelationTable> EventToCommentRelations { get; set; }
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