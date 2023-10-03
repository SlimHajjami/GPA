using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace GPA.Models
{
    public class ParametresDepenseModel
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Required]
        [DisplayName("Client")]
        [UIHint("CustomGridForeignKey")]
        public int ClientId { get; set; }

        [Required]
        [DisplayName("Dépense")]
        public string Depense { get; set; }
    }
}
