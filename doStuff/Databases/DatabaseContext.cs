using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using doStuff.Models.DatabaseModels;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace doStuff.Databases
{
    public class DatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<GroupToUserRelation> GroupToUserRelations { get; set; }
        public DbSet<UserToUserRelation> UserToUserRelations { get; set; }
        public DbSet<EventToUserRelation> EventToUserRelations { get; set; }
        public DbSet<EventToCommentRelation> EventToCommentRelations { get; set; }
        public DbSet<GroupToEventRelation> GroupToEventRelations { get; set; }
        public DatabaseContext()
            : base("DefaultConnection")
        {
        }
    }
}