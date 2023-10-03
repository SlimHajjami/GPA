using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GPA.DAL
{
    using GPA.DAL;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Reflection;

    public partial class Entities : DbContext
    {

        public Entities(int userId)
            : base("name=Entities")
        {
            UserId = userId;
        }
       
        public int UserId
        {
            get;
            private set;
        }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch(Exception ex)
            {
                string exception = ex.InnerException.ToString();
                return 0;
            }
        }
    }
}
