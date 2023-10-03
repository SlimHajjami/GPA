using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GPA.Models
{
    public class EntretienProgrammeModel
    {
        [ScaffoldColumn(false)]
        public int IdEntretienProgramme { get; set; }

        [Required]
        [DisplayName("Client")]
        [UIHint("CustomGridForeignKey")]
        public int ClientId { get; set; }

        [Required]
        [DisplayName("Véhicule")]
        [UIHint("CustomGridForeignKey")]
        public int VehiculeId { get; set; }

        [Required]
        [DisplayName("Entretien Maître")]
        [UIHint("CustomGridForeignKey")]
        public int EntretienMaitreId { get; set; }


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


        [DisplayName("Exécuté")]
        public string Execution { get; set; }

        [DisplayName("Exécuté le")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> DateExecution { get; set; }

        [DisplayName("Executé à")]
        public Nullable<int> KilometrageExecution { get; set; }


        [DisplayName("Prochain Entretien")]
        public string Prochain { get; set; }

        [DisplayName("Prochaine Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> ProchaineDate { get; set; }

        [DisplayName("Prochaine Kilométrage")]
        public Nullable<int> ProchaineKilometrage { get; set; }
    }
}
