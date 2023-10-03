using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GPA.Models
{
    public class DepartementsAccessRightsModel
    {
        public DepartementsAccessRightsModel()
        {
            Saisie = new List<int>();
            Visualisation = new List<int>();
            Validation = new List<int>();
        }
        public List<int> Saisie { get; set; }
        public List<int> Visualisation { get; set; }
        public List<int> Validation { get; set; }
    }
}
