using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GPA.Models
{
    public class EntretienModel
    {
        [ScaffoldColumn(false)]
        public int IdEntretien { get; set; }

        [Required]
        [DisplayName("Client")]
        [UIHint("CustomGridForeignKey")]
        public int ClientId { get; set; }

        [Required]
        [DisplayName("Véhicule")]
        [UIHint("CustomGridForeignKey")]
        public int VehiculeId { get; set; }

        [Required]
        [DisplayName("Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public System.DateTime DateEntretien { get; set; }

        [DisplayName("Fournisseur")]
        [UIHint("CustomGridForeignKey")]
        public Nullable<int> FournisseurId { get; set; }

        [DisplayName("Description")]
        public string DescriptionEntretien { get; set; }

        [Required]
        [DisplayName("Coût")]
        public decimal CoutEntretien { get; set; }

        [DisplayName("Kilométrage")]
        public Nullable<int> KilometrageEntretien { get; set; }

        [DisplayName("Note")]
        public string NoteEntretien { get; set; }
    }
}
