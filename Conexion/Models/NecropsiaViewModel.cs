using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Conexion.Models
{
    public class NecropsiaViewModel
    {
        public int id { get; set; }
        public string lugar { get; set; }
        public string fecha { get; set; }
        public string marcaje { get; set; }
        public string nombrePropio { get; set; }
        public string nombreComun { get; set; }
        public int idVeterinario { get; set; }
        public int idEncargado { get; set; }
        public string hora { get; set; }
        public string antecedentes { get; set; }
        public string diagnosticoMuerte { get; set; }
        public string estadoFisico { get; set; }
        public string lesiones { get; set; }
        public string toracica { get; set; }
        public string abdominal { get; set; }
        public string muestras { get; set; }


    }
}