using System;
using System.Collections.Generic;

namespace GPA.Models
{
    public class RapportEntretienReparationMensuelModel
    {
        public int ClientId { get; set; }
        public string Client { get; set; }
        public int VehiculeId { get; set; }
        public string Vehicule { get; set; }
        public int? DepartementId { get; set; }
        public string Departement { get; set; }
        public decimal Kilometrage { get; set; }
        public decimal Consommation { get; set; }
        public decimal Entretien { get; set; }
        public decimal Reparation { get; set; }
        public decimal CoutTotal { get; set; }
        public decimal CoutKM { get; set; }
        public decimal C100KM { get; set; }
        public decimal ER100KM { get; set; }
        public string Conducteur { get; set; }
    }
}
