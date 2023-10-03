using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GPA.Models
{
    public class DepenseModel
    {
        [ScaffoldColumn(false)]
        public int IdDepense { get; set; }

        [Required]
        [DisplayName("Type")]
        [UIHint("CustomGridForeignKey")]
        public int TypeId { get; set; }

        [ScaffoldColumn(false)]
        public Nullable<int> EntretienId { get; set; }

        [ScaffoldColumn(false)]
        public Nullable<int> ReparationId { get; set; }

        [Required]
        [DisplayName("Quantité")]
        public decimal QuantiteDepense { get; set; }

        [Required]
        [DisplayName("Coût Unitaire")]
        public decimal CoutUnitaireDepense { get; set; }

        [DisplayName("Pièce")]
        [UIHint("CustomGridForeignKey")]
        public Nullable<int> PieceId { get; set; }

        [DisplayName("Expiration Garantie")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> ExpirationGarantie { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Note")]
        public string NoteDepense { get; set; }
    }
}
