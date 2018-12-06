using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Conexion.Models
{
    public class EjemplarViewModel
    {
        public string marcaje { get; set; }
        public int idAnimal { get; set; }
        public string nombreComun { get; set;}
        public string fechaNacimiento { get; set; }
        public string fechaAlta {get; set;}
        public string sexo { get; set; }
        public string nombrePropio { get; set; }
    }
}