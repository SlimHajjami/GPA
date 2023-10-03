using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GPA.Models
{
    public class VehiculeAcquisitionPaiementModel
    {
        [ScaffoldColumn(false)]
        public int IdPaiement { get; set; }

        [ScaffoldColumn(false)]
        public int AcquisitionId { get; set; }

        [DisplayName("Référence")]
        public string ReferencePaiement { get; set; }

        [Required]
        [DisplayName("Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DatePaiement { get; set; }

        [Required]
        [DisplayName("Montant")]
        public decimal MontantPaiement { get; set; }

        [DisplayName("Note")]
        public string NotePaiement { get; set; }
    }
}
