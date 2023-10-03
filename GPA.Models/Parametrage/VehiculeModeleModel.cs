using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GPA.Models
{
    public class VehiculeModeleModel
    {
        [ScaffoldColumn(false)]
        public int IdModele { get; set; }

        [Required] 
        [DisplayName("Modèle")]
        public string NomModele { get; set; }

        [Required]
        [ScaffoldColumn(false)]
        public int CodeRefMarque { get; set; }

    }
}
