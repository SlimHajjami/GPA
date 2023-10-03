using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GPA.Models
{
    public class UtilisateurModel
    {
        [ScaffoldColumn(false)]
        public int IdUtilisateur { get; set; }

        [Required]
        [DisplayName("Rôle")]
        [UIHint("CustomGridForeignKey")]
        public int RoleId { get; set; }

        [Required]
        [DisplayName("Client")]
        [UIHint("CustomGridForeignKey")]
        public int ClientId { get; set; }

        [Required]
        [DisplayName("Nom")]
        public string NomUtilisateur { get; set; }

        [Required]
        [DisplayName("Prénom")]
        public string PrenomUtilisateur { get; set; }

        [Required]
        [DisplayName("Login")]
        public string LoginUtilisateur { get; set; }

        [Required]
        [DisplayName("Mot de passe")]
        public string PasswordUtilisateur { get; set; }

        [Required]
        [DisplayName("Email")]
        public string EmailUtilisateur { get; set; }

        [DisplayName("Fonction")]
        public string FonctionUtilisateur { get; set; }
    }
}
