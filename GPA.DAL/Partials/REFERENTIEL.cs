using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GPA.DAL
{
    public partial class REFERENTIEL : AuditableEntity
    {
        public override int GetId()
        {
            return this.ID_REFERENTIELS;
        }
    }
}
