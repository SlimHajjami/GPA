
namespace GPA.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    public class ReferentielViewModel
    {
        
        [ScaffoldColumn(false)]
        public int ID_REFERENTIELS { get; set; }
         [ScaffoldColumn(false)]
        public string KEY_REF { get; set; }
         [ScaffoldColumn(false)]
        public int CODE_REF { get; set; }

        [DisplayName("Valeur")]
        public string VALUE { get; set; }
    }

    
}