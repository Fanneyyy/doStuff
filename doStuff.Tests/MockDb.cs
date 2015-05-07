using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using doStuff.Databases;
using System.Data.Entity;
using doStuff.Models.DatabaseModels;

namespace doStuff.Tests
{
    class MockDb : IDataContext
    {
        public IDbSet<User> Users { get; set; }
        public IDbSet<Group> Groups { get; set; }
        public IDbSet<Event> Events { get; set; }
        public IDbSet<Comment> Comments { get; set; }
        public IDbSet<GroupToUserRelation> GroupToUserRelations { get; set; }
        public IDbSet<GroupToEventRelation> GroupToEventRelations { get; set; }
        public IDbSet<UserToUserRelation> UserToUserRelations { get; set; }
        public IDbSet<EventToUserRelation> EventToUserRelations { get; set; }
        public IDbSet<EventToCommentRelation> EventToCommentRelations { get; set; }
        public MockDb()
        {
            Users = new InMemoryDbSet<User>();
            Groups = new InMemoryDbSet<Group>();
            Events = new InMemoryDbSet<Event>();
            Comments = new InMemoryDbSet<Comment>();
            GroupToUserRelations = new InMemoryDbSet<GroupToUserRelation>();
            GroupToEventRelations = new InMemoryDbSet<GroupToEventRelation>();
            UserToUserRelations = new InMemoryDbSet<UserToUserRelation>();
            EventToUserRelations = new InMemoryDbSet<EventToUserRelation>();
            EventToCommentRelations = new InMemoryDbSet<EventToCommentRelation>();
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
