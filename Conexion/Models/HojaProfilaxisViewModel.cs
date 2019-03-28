using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Conexion.Models
{
    public class HojaProfilaxisViewModel
    {
        public int id { get; set; }
        
        public string lugar { get; set; }
       
        public string fecha { get; set; }
        
        public string tratamiento { get; set; }
        public string observaciones { get; set; }  
        public string fechaAplicacion { get; set; }
        public string fechaProxima { get; set; }

        public string marcaje { get; set; }
        public string nombrePropio { get; set; }
        public string nombreComun { get; set; }
        public int idVeterinario { get; set; }


    }
}