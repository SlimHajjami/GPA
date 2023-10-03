using System;
using System.Collections.Generic;

namespace GPA.Models
{
    public class RapportDepensesModel
    {
        public int ClientId { get; set; }
        public string Client { get; set; }
        public int VehiculeId { get; set; }
        public string Vehicule { get; set; }
        public int? DepartementId { get; set; }
        public string Departement { get; set; }
        public decimal Entretien { get; set; }
        public decimal Reparation { get; set; }
        public decimal Carburant { get; set; }
        public string Conducteur { get; set; }
    }
}
