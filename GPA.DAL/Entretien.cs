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
    
    public partial class Entretien
    {
        public int IdEntretien { get; set; }
        public int ClientId { get; set; }
        public int VehiculeId { get; set; }
        public System.DateTime DateEntretien { get; set; }
        public Nullable<int> FournisseurId { get; set; }
        public string DescriptionEntretien { get; set; }
        public decimal CoutEntretien { get; set; }
        public Nullable<int> KilometrageEntretien { get; set; }
        public string NoteEntretien { get; set; }
    }
}