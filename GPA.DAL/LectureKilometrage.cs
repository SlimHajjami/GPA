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
    
    public partial class LectureKilometrage
    {
        public int IdLectureKilometrage { get; set; }
        public int ClientId { get; set; }
        public int VehiculeId { get; set; }
        public System.DateTime DateLectureKilometrage { get; set; }
        public int Kilometrage { get; set; }
        public string Note { get; set; }
    }
}
