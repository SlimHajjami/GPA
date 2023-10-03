using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GPA.Models
{
    public class GISGPAModel
    {
        [ScaffoldColumn(false)]
        public int IdGisGpa { get; set; }

        [Required]
        [DisplayName("Client")]
       // [UIHint("CustomGridForeignKey")]
        public int ClientId { get; set; }

        [Required]
        public int VehiculeGPAId { get; set; }

        //[Required]
        [DisplayName("Véhicule GPA")]
        public string VehiculeGPAMat { get; set; }

        [Required]
        [DisplayName("Véhicule GIS")]
        public string VehiculeGISMat { get; set; }

        public int? VehiculeGISId { get; set; }

        public int? DepartementId{ get; set; }

    }
}
