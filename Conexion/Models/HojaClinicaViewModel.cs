using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Conexion.Models
{
    public class HojaClinicaViewModel
    {
        public int id { get; set; }
        public string lugar { get; set; }
        public string fecha { get; set; }
        public string antecedentes {get; set;}
        public string diagnostico { get; set; }
        public string tratamiento { get; set; }
        public string fechaAplicacion { get; set; }
        public string observaciones { get; set; }
        public string fechaAlta { get; set; }
        public string marcaje { get; set; }
        public string nombrePropio { get; set; }
        public string nombreComun { get; set; }

        public int idVeterinario { get; set; }
         
         

    }
}