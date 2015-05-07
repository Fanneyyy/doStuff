using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using doStuff.Databases;
using doStuff.Models.DatabaseModels;
using System.Data.Entity;

namespace doStuff.Tests
{
    class MockDb : IDostuffDataContext
    {
        public IDbSet<UserTable> Users { get; set; }
        public IDbSet<GroupTable> Groups { get; set; }
        public IDbSet<EventTable> Events { get; set; }
        public IDbSet<CommentTable> Comments { get; set; }
        public IDbSet<GroupToUserRelationTable> GroupToUserRelations { get; set; }
        public IDbSet<FriendShipRelationTable> FriendShipRelations { get; set; }
        public IDbSet<EventToUserRelationTable> EventToUserRelations { get; set; }
        public IDbSet<EventToCommentRelationTable> EventToCommentRelations { get; set; }
        public MockDb()
        {
            Users = new InMemoryDbSet<UserTable>();
            Groups = new InMemoryDbSet<GroupTable>();
            Events = new InMemoryDbSet<EventTable>();
            Comments = new InMemoryDbSet<CommentTable>();
            GroupToUserRelations = new InMemoryDbSet<GroupToUserRelationTable>();
            FriendShipRelations = new InMemoryDbSet<FriendShipRelationTable>();
            EventToUserRelations = new InMemoryDbSet<EventToUserRelationTable>();
            EventToCommentRelations = new InMemoryDbSet<EventToCommentRelationTable>();
        }
        public int SaveChanges()
        {
            // Pretend that each entity gets a database id when we hit save.
            int changes = 0;

            return changes;
        }
        
		public void Dispose()
		{
			// Do nothing!
		}
	}
}
