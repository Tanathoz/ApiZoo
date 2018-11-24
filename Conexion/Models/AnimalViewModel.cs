using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Conexion.Models
{
    public class AnimalViewModel
    {
        public int id { get; set; }
        public string nombreCientifico { get; set; }
        public string nombreComun { get; set; }
        public string familia { get; set; }
        public string clase { get; set; }
        public string orden { get; set; }
        public string especie { get; set; }
        public string habitat { get; set; }
        public string gestacion { get; set; }
        public string camada { get; set; }
        public string longevidad { get; set; }
        public string peso { get; set; }
        public string ubicacionGeografica { get; set; }
        public string alimentacion { get; set; }


    }

}