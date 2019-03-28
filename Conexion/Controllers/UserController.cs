using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using MySql.Data.MySqlClient;
using Conexion.Models;


namespace Conexion.Controllers
{
    public class UserController : ApiController
    {

        
        public IHttpActionResult Login(string email, string nombre)
        {
            if (!ModelState.IsValid)
                return BadRequest("Modelo de datos no válido");

            UserViewModel datos =null;
            using (MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                conexion.Open();
                string consultar = "select name , email from users where email='" + email.ToString() +"' and name='" + nombre.ToString()+ " '  ;" ;
                MySqlCommand query = new MySqlCommand(consultar, conexion);
                using (var reader = query.ExecuteReader())
                {
                    try
                    {
                        reader.Read();
                        datos = new UserViewModel()
                        {
                            nombre = reader["name"].ToString(),
                            email = reader["email"].ToString()
                        };
                    }catch(Exception e)
                    {
                        return BadRequest("usuario o contraseña incorrectos"+ e.Message );
                    }
                }

            }

            if (datos == null)
            {
                return NotFound();
            }else
            {
                return Ok(datos);
            }

        }

    }
}
