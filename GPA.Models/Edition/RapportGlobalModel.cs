using System;
using System.Collections.Generic;

namespace GPA.Models
{
    public class RapportGlobalModel
    {
        public int ClientId { get; set; }
        public string Client { get; set; }
        public int DepartementId { get; set; }
        public string Departement { get; set; }
        public int NbVehicules { get; set; }
        public decimal C1 { get; set; }
        public decimal C2 { get; set; }
        public decimal C3 { get; set; }
        public decimal C4 { get; set; }
        public decimal E1 { get; set; }
        public decimal E2 { get; set; }
        public decimal E3 { get; set; }
        public decimal E4 { get; set; }
        public decimal R1 { get; set; }
        public decimal R2 { get; set; }
        public decimal R3 { get; set; }
        public decimal R4 { get; set; }
    }
}
