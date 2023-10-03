using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace GPA.Models
{
    public class CarburantCoutUnitaireModel
    {
        [ScaffoldColumn(false)]
        public int IdCarburant { get; set; }

        [DisplayName("Coût/Litre")]
        public decimal? CoutUnitaireCarburant { get; set; }

        [DisplayName("Type Carburant")]
        public string TypeCarburant { get; set; }

        public int CodeCarburant { get; set; }
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DisplayName("Date d'application du tarif")]
        public System.DateTime DateChangementTarif { get; set; }
    }
}
