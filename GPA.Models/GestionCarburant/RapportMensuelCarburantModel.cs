using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GPA.Models
{
    public class RapportMensuelCarburantModel
    {
        public int ClientId { get; set; }
        public string Client { get; set; }
        public int VehiculeId { get; set; }
        public string Vehicule { get; set; }
        public int? DepartementId { get; set; }
        public string Departement { get; set; }
        public decimal Consommation { get; set; }
        public decimal KilometragePR { get; set; }
        public decimal Kilometrage { get; set; }
        public decimal Cout { get; set; }
        public string Conducteur { get; set; }

    }
}
