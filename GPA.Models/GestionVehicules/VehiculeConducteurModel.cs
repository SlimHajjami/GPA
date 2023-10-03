using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GPA.Models
{
    public class VehiculeConducteurModel
    {
        [ScaffoldColumn(false)]
        public int IdConducteur { get; set; }

        [Required]
        [DisplayName("Client")]
        [UIHint("CustomGridForeignKey")]
        public int ClientId { get; set; }

        [Required]
        [DisplayName("Nom")]
        public string NomConducteur { get; set; }

        [Required]
        [DisplayName("Prénom")]
        public string PrenomConducteur { get; set; }

        [ScaffoldColumn(false)]
        public string NomPrenomConducteur { get; set; }

        [DisplayName("Date Naissance")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> DateNaissanceConducteur { get; set; }

        [DisplayName("Date Embauche")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> DateEmbaucheConducteur { get; set; }

        [DisplayName("N° Permis")]
        public string NumeroPermisConducteur { get; set; }

        [DisplayName("Adresse")]
        public string AdresseConducteur { get; set; }

        [DisplayName("Ville")]
        public string VilleConducteur { get; set; }

        [DisplayName("Code Postal")]
        public string CodePostalConducteur { get; set; }

        [DisplayName("Tél1")]
        public string Tel1Conducteur { get; set; }

        [DisplayName("Tél2")]
        public string Tel2Conducteur { get; set; }

        [DisplayName("Email")]
        public string EmailConducteur { get; set; }
    }
}
