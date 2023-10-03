using System;
using System.Collections.Generic;

namespace GPA.Models
{
    public class RapportConsommationCarburantMensuelModel
    {
        public int ClientId { get; set; }
        public string Client { get; set; }
        public int VehiculeId { get; set; }
        public string Vehicule { get; set; }
        public int? DepartementId { get; set; }
        public string Departement { get; set; }
        public decimal Kilometrage { get; set; }
        public decimal KilometragePR { get; set; }
        public decimal Consommation { get; set; }
        public decimal ConsommationPR { get; set; }
        public decimal C100KM { get; set; }
        public decimal C100KMPR { get; set; }
        public string Conducteur { get; set; }
    }
}
