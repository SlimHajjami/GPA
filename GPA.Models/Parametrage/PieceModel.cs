using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GPA.Models
{
    public class PieceModel
    {
        [ScaffoldColumn(false)]
        public int IdPiece { get; set; }

        [Required]
        [DisplayName("Client")]
        [UIHint("CustomGridForeignKey")]
        public int ClientId { get; set; }

        [DisplayName("Numéro")]
        public string NumeroPiece { get; set; }

        [Required]
        [DisplayName("Nom")]
        public string NomPiece { get; set; }

        [Required]
        [DisplayName("Prix")]
        public decimal PrixPiece { get; set; }

        [Required]
        [DisplayName("Date Prix")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public System.DateTime DatePrixPiece { get; set; }

        [DisplayName("Fournisseur")]
        [UIHint("CustomGridForeignKey")]
        public Nullable<int> FournisseurId { get; set; }

        [DisplayName("Constructeur")]
        public string Constructeur { get; set; }
    }
}
