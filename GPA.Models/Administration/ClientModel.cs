using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GPA.Models
{
    public class ClientModel
    {
        [ScaffoldColumn(false)]
        public int IdClient { get; set; }

        [Required]
        [DisplayName("Client")]
        public string NomClient { get; set; }

        [DisplayName("Adresse")]
        public string AdresseClient { get; set; }

        [DisplayName("Ville")]
        public string VilleClient { get; set; }

        [DisplayName("Code Postal")]
        public string CodePostalClient { get; set; }

        [DisplayName("Contact")]
        public string ContactClient { get; set; }

        [DisplayName("Tél1")]
        public string Tel1Client { get; set; }

        [DisplayName("Tél2")]
        public string Tel2Client { get; set; }

        [DisplayName("Fax")]
        public string FaxClient { get; set; }

        [DisplayName("Site Web")]
        public string SiteWebClient { get; set; }

        [DisplayName("Email")]
        public string EmailClient { get; set; }

        [DisplayName("Paramètres GIS")]
        public string ParametresGis { get; set; }
    }
}
