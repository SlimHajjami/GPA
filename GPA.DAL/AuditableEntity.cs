using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GPA.DAL
{
    public interface IAuditableEntity
    {
        int GetId();
    }

    public abstract class AuditableEntity : IAuditableEntity
    {
        public abstract int GetId();
    }
}
