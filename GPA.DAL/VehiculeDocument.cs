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
    
    public partial class VehiculeDocument
    {
        public int IdDocuments { get; set; }
        public int VehiculeId { get; set; }
        public Nullable<int> FournisseurAssuranceId { get; set; }
        public Nullable<System.DateTime> DateAssurance { get; set; }
        public Nullable<System.DateTime> ExpirationAssurance { get; set; }
        public Nullable<int> RappelAssurance { get; set; }
        public string NoteAssurance { get; set; }
        public Nullable<System.DateTime> DateTaxeCirculation { get; set; }
        public Nullable<System.DateTime> ExpirationTaxeCirculation { get; set; }
        public Nullable<int> RappelTaxeCirculation { get; set; }
        public string NoteTaxeCirculation { get; set; }
        public Nullable<System.DateTime> DerniereVisiteTechnique { get; set; }
        public Nullable<System.DateTime> ProchaineVisiteTechnique { get; set; }
        public Nullable<int> RappelVisiteTechnique { get; set; }
        public string NoteVisiteTechnique { get; set; }
    }
}
