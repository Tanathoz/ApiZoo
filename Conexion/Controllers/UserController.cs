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


        public IHttpActionResult GetAllUser()
        {
            List<UserViewModel> lstUsers = new List<UserViewModel>();
            using (MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                conexion.Open();
                string consulta = "Select id, name, email, password from users ;";
                MySqlCommand query = new MySqlCommand(consulta, conexion);
                using (var reader = query.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lstUsers.Add(new UserViewModel
                        {
                            id = Convert.ToInt32(reader["id"].ToString()),
                            nombre = reader["name"].ToString(),
                            email = reader["email"].ToString(),
                            password = reader["password"].ToString()

                        });
                    }
                }

                if (lstUsers.Count == 0)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(lstUsers);
                }

            }
        }


        public IHttpActionResult GetLogin(string email, string password)
        {
            if (!ModelState.IsValid)
                return BadRequest("Modelo de datos no válido");

            UserViewModel datos =null;
            using (MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                conexion.Open();
                string consultar = "select name , email from users where email='" + email.ToString() +"' and password='" + password.ToString()+ " '  ;" ;
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


        public IHttpActionResult PostUser(UserViewModel user)
        {
            string password = Encriptar(user.password);
            if (!ModelState.IsValid)
                return BadRequest("Datos invalidos");

            using (MySqlConnection conexion = ConexionBase.GetDBConnection())
            {

                conexion.Open();
                string insert = "insert into users (id, name, email, password) values ('" + user.id + "', '" + user.nombre + "', '" + user.email + "', '" + password + "');";
                MySqlCommand query = new MySqlCommand(insert, conexion);
                MySqlDataReader myReader;
                myReader = query.ExecuteReader();
                conexion.Close();
                return Ok();
            }
        }

        public  string Encriptar(string cadena)
        {
            string result = string.Empty;
            byte[] encryted = System.Text.Encoding.Unicode.GetBytes(cadena);
            result = Convert.ToBase64String(encryted);
            return result;
        }

        public string Desencriptar (string condificada)
        {
            string result = string.Empty;
            byte[] decryted = Convert.FromBase64String(condificada);
            result = System.Text.Encoding.Unicode.GetString(decryted);
            return result;
        }

    }
}
