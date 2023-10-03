using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GPA.Models
{
    public class AccessRightDepartementModel
    {
        public int IdAccessRightDepartement { get; set; }
        public int DepartementId { get; set; }
        public int UtilisateurId { get; set; }
        public string Libelle { get; set; }
        public bool Visualisation { get; set; }
        public bool Saisie { get; set; }
        public bool Validation { get; set; }
    }
}
