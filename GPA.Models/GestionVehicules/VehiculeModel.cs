using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace GPA.Models
{
    public class VehiculeModel
    {
        [ScaffoldColumn(false)]
        public int IdVehicule { get; set; }

        [Required]
        [DisplayName("Client")]
        [UIHint("CustomGridForeignKey")]
        public int ClientId { get; set; }

        [Required]
        [DisplayName("Matricule")]
        public string MatriculeVehicule { get; set; }

        [DisplayName("Marque")]
        [UIHint("CustomGridForeignKey")]
        public int? codeRefMarque { get; set; }

        [DisplayName("Marque")]
        public string MarqueVehicule { get; set; }

        [DisplayName("Modèle")]
        [UIHint("CustomGridForeignKey")]
        public int? IdModele { get; set; }

        [DisplayName("Modèle")]
        public string ModeleVehicule { get; set; }

        [DisplayName("Couleur")]
        public string CouleurVehicule { get; set; }

        [DisplayName("N° Série")]
        public string NumeroSerieVehicule { get; set; }

        [DisplayName("Mise Circulation")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> MiseCirculation { get; set; }
        [Required]
        //[JsonProperty("allowUploading", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        //[DefaultValue(2)]
        //[JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        [DisplayName("Type Carburant")]
        [UIHint("CustomGridForeignKey")]
        public Nullable<int> TypeCarburantId { get; set; }

        [ScaffoldColumn(false)]
        public string TypeCarburant { get; set; }

        [DisplayName("Type")]
        [UIHint("CustomGridForeignKey")]
        public Nullable<int> TypeVehiculeId { get; set; }

        [ScaffoldColumn(false)]
        public string TypeVehicule { get; set; }

        [DisplayName("Département")]
        [UIHint("CustomGridForeignKey")]
        public Nullable<int> DepartementId { get; set; }

        [ScaffoldColumn(false)]
        public string Departement { get; set; }

        [DisplayName("Conducteur")]
        [UIHint("CustomGridForeignKey")]
        public Nullable<int> ConducteurId { get; set; }

        [ScaffoldColumn(false)]
        public string Conducteur { get; set; }

        [DisplayName("Actif")]
        public bool IsActif { get; set; }
    }

    public class VehiculeColumns
    {
        public string Matricule { get; set; }
        public string Marque { get; set; }
        public string Modele { get; set; }
        public string Couleur { get; set; }
        public string NumeroSerie { get; set; }
        public string MiseCirculation { get; set; }
        public string Carburant { get; set; }
        public string Type { get; set; }
        public string Departement { get; set; }
        public string Conducteur { get; set; }
    }
}
