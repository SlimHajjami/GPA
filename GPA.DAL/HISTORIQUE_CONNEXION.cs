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
    
    public partial class HISTORIQUE_CONNEXION
    {
        public int ID { get; set; }
        public int USER_ID { get; set; }
        public System.DateTime LogIn_Date { get; set; }
        public Nullable<System.DateTime> LogOut_Date { get; set; }
        public string AdresseIP { get; set; }
    }
}
