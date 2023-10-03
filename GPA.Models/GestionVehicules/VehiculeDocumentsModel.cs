using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GPA.Models
{
    public class VehiculeDocumentsModel
    {
        [ScaffoldColumn(false)]
        public int IdDocuments { get; set; }

        [DisplayName("Client")]
        public string Client { get; set; }

        [Required]
        [DisplayName("Véhicule")]
        [UIHint("CustomGridForeignKey")]
        public int VehiculeId { get; set; }

        [ScaffoldColumn(false)]
        public string Vehicule { get; set; }

        [ScaffoldColumn(false)]
        public string Departement { get; set; }


        [DisplayName("Assureur")]
        [UIHint("CustomGridForeignKey")]
        public Nullable<int> FournisseurAssuranceId { get; set; }

        [ScaffoldColumn(false)]
        public string FournisseurAssurance { get; set; }

        [DisplayName("Date Assurance")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<DateTime> DateAssurance { get; set; }

        [DisplayName("Expiration Assurance")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<DateTime> ExpirationAssurance { get; set; }

        [DisplayName("Rappel Avant (jours)")]
        public Nullable<int> RappelAssurance { get; set; }

        [DisplayName("Note")]
        public string NoteAssurance { get; set; }


        [DisplayName("Date Taxe")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<DateTime> DateTaxeCirculation { get; set; }

        [DisplayName("Expiration Taxe")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<DateTime> ExpirationTaxeCirculation { get; set; }

        [DisplayName("Rappel Avant (jours)")]
        public Nullable<int> RappelTaxeCirculation { get; set; }

        [DisplayName("Note")]
        public string NoteTaxeCirculation { get; set; }


        [DisplayName("Dernière Visite")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<DateTime> DerniereVisiteTechnique { get; set; }

        [DisplayName("Prochaine Visite")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<DateTime> ProchaineVisiteTechnique { get; set; }

        [DisplayName("Rappel Avant (jours)")]
        public Nullable<int> RappelVisiteTechnique { get; set; }

        [DisplayName("Note")]
        public string NoteVisiteTechnique { get; set; }
    }
}
