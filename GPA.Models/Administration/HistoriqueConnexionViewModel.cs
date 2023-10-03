using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GPA.Models
{
    public class HistoriqueConnexionViewModel
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }
        [DisplayName("ID_Utilisateur")]
        public int UserId { get; set; }

        public string Utilisateur { get; set; }

        public string Role { get; set; }

        public DateTime DateLogIn { get; set; }

        [DisplayName("Date de Connexion")]
        public string DateToDisplay { get; set; }
     
        public Nullable<System.DateTime> DateLogOut { get; set; }

        [DisplayName("Temps d'activité")]
        public string TempsActivite { get; set; }

        [DisplayName("Adresse IP")]
        public string AdresseIP { get; set; }

        public String Etat { get; set; }
    }
}
