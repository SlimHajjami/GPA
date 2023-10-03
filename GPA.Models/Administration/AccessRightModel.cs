using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GPA.Models
{
    public class AccessRightModel
    {
        [ScaffoldColumn(false)]
        public int IdAccessRight { get; set; }

        [UIHint("CustomGridForeignKey")]
        public int RoleId { get; set; }

        public int AccessRight { get; set; }
    }
}
