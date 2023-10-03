using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace GPA.Models
{
    public class AutresDepenseModel
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Required]
        [DisplayName("Dépenses")]
        [UIHint("CustomGridForeignKey")]
        public int DepenseId { get; set; }

        [DisplayName("Véhicule")]
        [UIHint("CustomGridForeignKey")]
        public int? VehiculeId { get; set; }

        [Required]
        [DisplayName("Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateDepenses { get; set; }

        [DisplayName("Kilomètrage")]
        public int? Kilometrage { get; set; }

        [DisplayName("Notes")]
        public string Note { get; set; }

        [Required]
        [DisplayName("Montant")]
        public decimal CoutTotal { get; set; }
    }
}
