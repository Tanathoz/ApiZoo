﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Conexion.Models
{
    public class UserViewModel
    {
       public int id { get; set; }
       public string nombre { get; set; }
       public string email { get; set; }
       public string password { get; set; }

    }
}