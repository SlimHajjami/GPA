using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GPA.Models
{
    public class ModeleC100KMModel
    {
        [ScaffoldColumn(false)]
        public int IdModeleC100KM { get; set; }
        
        [Required]
        [DisplayName("Client")]
        [UIHint("CustomGridForeignKey")]
        public int ClientId { get; set; }

        [Required]
        [DisplayName("Modèle")]
        public int ModeleId { get; set; }

        [DisplayName("Modèle")]
        public string Modele { get; set; }

        [Required]
        [DisplayName("C/100KM")]
        public decimal C100KM { get; set; }
    }
}
