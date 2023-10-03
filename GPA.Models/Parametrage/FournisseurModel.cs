using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GPA.Models
{
    public class FournisseurModel
    {
        [ScaffoldColumn(false)]
        public int IdFournisseur { get; set; }

        [Required]
        [DisplayName("Client")]
        [UIHint("CustomGridForeignKey")]
        public int ClientId { get; set; }

        [Required]
        [DisplayName("Type")]
        [UIHint("CustomGridForeignKey")]
        public int TypeFournisseur { get; set; }

        [Required]
        [DisplayName("Fournisseur")]
        public string NomFournisseur { get; set; }

        [DisplayName("Adresse")]
        public string AdresseFournisseur { get; set; }

        [DisplayName("Ville")]
        public string VilleFournisseur { get; set; }

        [DisplayName("Code Postal")]
        public string CodePostalFournisseur { get; set; }

        [DisplayName("Contact")]
        public string ContactFournisseur { get; set; }

        [DisplayName("Tél1")]
        public string Tel1Fournisseur { get; set; }

        [DisplayName("Tél2")]
        public string Tel2Fournisseur { get; set; }

        [DisplayName("Fax")]
        public string FaxFournisseur { get; set; }

        [DisplayName("Site Web")]
        public string SiteWebFournisseur { get; set; }

        [DisplayName("Email")]
        public string EmailFournisseur { get; set; }
    }
}
