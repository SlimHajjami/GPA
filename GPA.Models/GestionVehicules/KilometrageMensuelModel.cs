using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace GPA.Models
{
    public class KilometrageMensuelModel
    {
        [ScaffoldColumn(false)]
        public int IdKilometrageMensuel { get; set; }

        [DisplayName("Client")]
   //     [UIHint("CustomGridForeignKey")]
        public int ClientId { get; set; }

        [DisplayName("Véhicule")]
        [UIHint("CustomGridForeignKey")]
        public int VehiculeId { get; set; }

        [Required]
        [DisplayName("Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/yyyy}")]
        public DateTime DateKilometrageMensuel { get; set; }

        [Required]
        [DisplayName("Kilométrage Mensuel")]
        public decimal Kilometrage { get; set; }
        public string Note { get; set; }
    }
}
