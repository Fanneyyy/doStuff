using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using doStuff.Models.DatabaseModels;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace doStuff.Databases
{
    public class DoStuffDatabase : DbContext
    {
        public DbSet<UserTable> Users { get; set; }
        public DbSet<GroupTable> Groups { get; set; }
        public DbSet<EventTable> Events { get; set; }
        public DbSet<CommentTable> Comments { get; set; }
        public DbSet<GroupToUserRelationTable> GroupToUserRelations { get; set; }
        public DbSet<FriendShipRelationTable> FriendShipRelations { get; set; }
        public DbSet<EventToUserRelationTable> EventToUserRelations { get; set; }
        public DbSet<EventToCommentRelationTable> EventToCommentRelations { get; set; }
    }
}