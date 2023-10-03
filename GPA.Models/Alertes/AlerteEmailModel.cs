using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GPA.Models
{
    public class AlerteEmailModel
    {
        [ScaffoldColumn(false)]
        public int IdAlerteEmail { get; set; }

        [Required]
        [DisplayName("Client")]
        [UIHint("CustomGridForeignKey")]
        public int ClientId { get; set; }

        [Required]
        [DisplayName("Email")]
        public string AlerteEmail { get; set; }

        [Required]
        [DisplayName("Type")]
        [UIHint("CustomGridForeignKey")]
        public int TypeAlerteId { get; set; }
    }
}
