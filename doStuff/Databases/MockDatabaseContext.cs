using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace doStuff.Databases
{
    public class MockDatabaseContext : DatabaseContext
    {
        public override int SaveChanges()
        {
            return 0;
        }

        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            return;
        }

        protected override void Dispose(bool disposing)
        {
            return;
        }
    }
}