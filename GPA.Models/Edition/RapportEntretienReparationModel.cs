using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GPA.Models
{
    public class RapportEntretienReparationModel
    {
        public int ClientId { get; set; }
        public string Client { get; set; }
        public int VehiculeId { get; set; }
        public string Vehicule { get; set; }
        public int? DepartementId { get; set; }
        public string Departement { get; set; }
        public int? Kilometrage { get; set; }
        public string Type { get; set; } //entretien ou reparation
        public string Date { get; set; }
        public string Description { get; set; }
        public int Quantite { get; set; }
        public decimal CoutUnite { get; set; }
        public decimal CoutTotal { get; set; }
    }
}
