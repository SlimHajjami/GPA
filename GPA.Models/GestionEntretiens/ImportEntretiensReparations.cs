using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GPA.Models
{
    public class ENT
    {
        public int vehicule { get; set; }
        public string date { get; set; }
        public string description { get; set; }
        public string type { get; set; }
        public string kilometrage { get; set; }
        public string note { get; set; }
    }

    public class DPN
    {
        public string type { get; set; }
        public string entretien { get; set; }
        public string cout { get; set; }
        public string quantite { get; set; }
    }

    public class VEH
    {
        public int id { get; set; }
        public string matricule { get; set; }
    }
}
