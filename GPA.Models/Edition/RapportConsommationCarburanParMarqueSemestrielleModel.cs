using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GPA.Models
{
    public class RapportConsommationCarburantParMarqueSemestrielleModel
    {
        public int VehiculeId { get; set; }
        public string Vehicule { get; set; }
        public string Modele { get; set; }
        public decimal C100KMModele { get; set; }
        public string Marque { get; set; }
        public string Conducteur { get; set; }
        public string MiseCirculation { get; set; }
        public decimal C1 { get; set; }
        public decimal C2 { get; set; }
        public decimal C3 { get; set; }
        public decimal C4 { get; set; }
        public decimal C5 { get; set; }
        public decimal C6 { get; set; }
    }
}
