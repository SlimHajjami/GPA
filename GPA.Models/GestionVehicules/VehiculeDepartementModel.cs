using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GPA.Models
{
    public class VehiculeDepartementModel
    {
        [ScaffoldColumn(false)]
        public int IdDepartement { get; set; }

        [Required]
        [DisplayName("Client")]
        [UIHint("CustomGridForeignKey")]
        public int ClientId { get; set; }

        [Required]
        [DisplayName("Nom")]
        public string NomDepartement { get; set; }
    }
}
