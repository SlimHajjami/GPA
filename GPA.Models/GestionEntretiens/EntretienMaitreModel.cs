using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GPA.Models
{
    public class EntretienMaitreModel
    {
        [ScaffoldColumn(false)]
        public int IdEntretienMaitre { get; set; }

        [Required]
        [DisplayName("Client")]
        [UIHint("CustomGridForeignKey")]
        public int ClientId { get; set; }

        [Required]
        [DisplayName("Description")]
        public string DescriptionEntretienMaitre { get; set; }

        [DisplayName("Répéter Toutes")]
        public string Repeter { get; set; }

        
        [DisplayName("Répéter Toutes")]
        public Nullable<int> RepeterNb { get; set; }

        
        [DisplayName("Répéter Période")]
        [UIHint("CustomGridForeignKey")]
        public Nullable<int> RepeterPeriode { get; set; }

        
        [DisplayName("Répéter Kilométrage")]
        public Nullable<int> RepeterKilometrage { get; set; }

        [DisplayName("Rappel Avant")]
        public string Rappel { get; set; }

        
        [DisplayName("Rappel Avant")]
        public Nullable<int> RappelNb { get; set; }

        
        [DisplayName("Rappel Période")]
        [UIHint("CustomGridForeignKey")]
        public Nullable<int> RappelPeriode { get; set; }

        
        [DisplayName("Rappel Kilométrage")]
        public Nullable<int> RappelKilometrage { get; set; }
    }
}
