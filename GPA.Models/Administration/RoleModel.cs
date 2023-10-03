using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace GPA.Models
{
    public class RoleModel
    {
        [ScaffoldColumn(false)]
        public int IdRole { get; set; }

        [DisplayName("Nom")]
        public string NomRole { get; set; }

        [DisplayName("Description")]
        public string DescriptionRole { get; set; }

        [ScaffoldColumn(false)]
        public Nullable<int> ProfileRole { get; set; }

        public Enumerators.AccessRights[] AccessRights { get; set; }
    }
}
