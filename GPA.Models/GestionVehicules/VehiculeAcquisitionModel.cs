using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GPA.Models
{
    public class VehiculeAcquisitionModel
    {
        [ScaffoldColumn(false)]
        public int IdAcquisition { get; set; }

        [DisplayName("Client")]
        public string Client { get; set; }

        [Required]
        [DisplayName("Véhicule")]
        [UIHint("CustomGridForeignKey")]
        public int VehiculeId { get; set; }

        [DisplayName("Location")]
        public Nullable<bool> IsLocation { get; set; }

        [DisplayName("Fournisseur")]
        [UIHint("CustomGridForeignKey")]
        public Nullable<int> FournisseurId { get; set; }

        [DisplayName("Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<DateTime> DateAcquisition { get; set; }

        [UIHint("Integer")]
        [DisplayName("Kilométrage")]
        public Nullable<int> KilometrageAcquisition { get; set; }

        [DisplayName("Prix")]
        public Nullable<decimal> PrixAcquisition { get; set; }

        [DisplayName("Reste à payer")]
        public Nullable<decimal> RestePayer { get; set; }

        [DisplayName("Expiration Garantie")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> ExpirationGarantie { get; set; }

        [DisplayName("Prix Vente")]
        public Nullable<decimal> PrixVente { get; set; }

        [DisplayName("Note")]
        public string NoteAcquisition { get; set; }
    }
}
