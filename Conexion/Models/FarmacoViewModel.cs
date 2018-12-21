using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using MySql.Data.MySqlClient;

namespace Conexion.Models
{
    public class FarmacoViewModel
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string via { get; set; }


    }
}