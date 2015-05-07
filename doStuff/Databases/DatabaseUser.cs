using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace doStuff.Databases
{
    public class DatabaseUser : DatabaseGroup
    {
        public DatabaseUser(IDostuffDataContext dbContext) : base(dbContext)
        {
        }
        //TODO
    }
}