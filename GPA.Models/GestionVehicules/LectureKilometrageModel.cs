using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GPA.Models
{
    public class LectureKilometrageModel
    {
        [ScaffoldColumn(false)]
        public int IdLectureKilometrage { get; set; }

        public int ClientId { get; set; }

        [DisplayName("Véhicule")]
        [UIHint("CustomGridForeignKey")]
        public int VehiculeId { get; set; }

        [Required]
        [DisplayName("Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateLectureKilometrage { get; set; }

        [Required]
        [DisplayName("Kilométrage")]
        public int Kilometrage { get; set; }

        [DisplayName("Kilométrage précédent")]
        public int KilometragePR { get; set; }

        public string Note { get; set; }
    }
}
