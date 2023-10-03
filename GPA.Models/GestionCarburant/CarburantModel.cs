using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GPA.Models
{
    public class CarburantModel
    {
        [ScaffoldColumn(false)]
        public int IdCarburant { get; set; }

        [Required]
        [DisplayName("Client")]
        [UIHint("CustomGridForeignKey")]
        public int ClientId { get; set; }

        [Required]
        [DisplayName("Véhicule")]
        [UIHint("CustomGridForeignKey")]
        public int VehiculeId { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [Required]
        [DisplayName("Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public System.DateTime DateCarburant { get; set; }

     //   [Required]
        [DisplayName("Quantité Litres")]
        public decimal QuantiteCarburant { get; set; }

        [Required]
        [DisplayName("Coût/Litre")]
        public decimal CoutUnitaireCarburant { get; set; }

      //  [Required]
        [DisplayName("Coût total")]
        public decimal CoutTotalCarburant { get; set; }

        [DisplayName("Fournisseur")]
        [UIHint("CustomGridForeignKey")]
        public Nullable<int> FournisseurId { get; set; }

        [DisplayName("Kilométrage Début")]
        public Nullable<int> KilometrageDebut { get; set; }

        [DisplayName("Dernière Lecture Odomètre")]
        public Nullable<int> OdometreActuel { get; set; }

        [DisplayName("Kilométrage Fin")]
        public Nullable<int> KilometrageFin { get; set; }

        [DisplayName("Note")]
        public string NoteCarburant { get; set; }
    }

    public class CarburantColumns
    {
        public string Matricule { get; set; }
        public string Date { get; set; }
        public string Quantite { get; set; }
        public string Cout { get; set; }
        public string CoutTotal { get; set; }
        public string Description { get; set; }
        public string Fournisseur { get; set; }
        public string KilometrageDebut { get; set; }
        public string KilometrageFin { get; set; }
        public string Note { get; set; }
    }
}
