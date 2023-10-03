using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GPA.Models;

namespace GPA.DAL
{
    public partial class AccessRight : AuditableEntity
    {
        public override int GetId()
        {
            return this.IdAccessRight;
        }

        
        public Enumerators.AccessRights AccessRightsAsEnum
        {
            get
            {
                return (Enumerators.AccessRights)AccessRight1;
            }
        }
    }
}
