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
    
    public partial class VehiculeAcquisition
    {
        public int IdAcquisition { get; set; }
        public int VehiculeId { get; set; }
        public Nullable<bool> IsLocation { get; set; }
        public Nullable<int> FournisseurId { get; set; }
        public Nullable<System.DateTime> DateAcquisition { get; set; }
        public Nullable<int> KilometrageAcquisition { get; set; }
        public Nullable<decimal> PrixAcquisition { get; set; }
        public Nullable<System.DateTime> ExpirationGarantie { get; set; }
        public Nullable<decimal> PrixVente { get; set; }
        public string NoteAcquisition { get; set; }
    }
}
