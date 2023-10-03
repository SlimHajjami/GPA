using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GPA.Models
{
    public class ReparationModel
    {
        [ScaffoldColumn(false)]
        public int IdReparation { get; set; }

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
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}")]
        public System.DateTime DateReparation { get; set; }

        [Required]
        [DisplayName("Mise en Service")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}")]
        public System.DateTime DateMiseService { get; set; }

        [Required]
        [DisplayName("Heures Arrêt")]
        public decimal HeuresArret { get; set; }

        [DisplayName("Fournisseur")]
        [UIHint("CustomGridForeignKey")]
        public Nullable<int> FournisseurId { get; set; }

        [DisplayName("Description")]
        public string DescriptionReparation { get; set; }

        [Required]
        [DisplayName("Coût")]
        public decimal CoutReparation { get; set; }

        [DisplayName("Kilométrage")]
        public Nullable<int> KilometrageReparation { get; set; }

        [DisplayName("Note")]
        public string NoteReparation { get; set; }
    }
}
