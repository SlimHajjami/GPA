//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GPA.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Utilisateur
    {
        public int IdUtilisateur { get; set; }
        public int RoleId { get; set; }
        public int ClientId { get; set; }
        public string NomUtilisateur { get; set; }
        public string PrenomUtilisateur { get; set; }
        public string LoginUtilisateur { get; set; }
        public string PasswordUtilisateur { get; set; }
        public string EmailUtilisateur { get; set; }
        public string FonctionUtilisateur { get; set; }
    }
}
