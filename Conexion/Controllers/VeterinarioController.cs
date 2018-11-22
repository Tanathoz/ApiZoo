using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Conexion.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace Conexion.Controllers
{
    public class VeterinarioController : ApiController
    {
        public IHttpActionResult GetAllVeterinarios()
        {

            Console.WriteLine("Getting Connection");
            MySqlConnection conn = ConexionBase.GetDBConnection();
            try
            {
                Console.WriteLine("Openning Conecction..");
                conn.Open();
                Console.WriteLine("Success connection");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error porpa" + e.Message);
            }

            //IList<VeterinarioModel> lstVeterinarios = null;
            //opcion origilan 
            List<VeterinarioModel> lstVeterinarios = new  List<VeterinarioModel>();
            using ( MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                conexion.Open();
                MySqlCommand query = new MySqlCommand("select id, nombre, apellidoPaterno, apellidoMaterno, sexo from veterinarios", conexion);
                using (var reader = query.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lstVeterinarios.Add(new VeterinarioModel()
                        {
                            id = Convert.ToInt32(reader["Id"]),
                            nombre = reader["nombre"].ToString(),
                            apellidoPaterno = reader["apellidoPaterno"].ToString(),
                            apellidoMaterno = reader["apellidoMaterno"].ToString(),
                            sexo = reader["sexo"].ToString()
                        });
                    }
                }
            }

            if (lstVeterinarios.Count == 0)
            {
                return NotFound();

            } else
            {
                return Ok(lstVeterinarios);
            }
        }


        public IHttpActionResult GetVeterinarioById(int id)
        {
            VeterinarioModel veterinario = null;
            using (MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                conexion.Open();
                MySqlCommand query = new MySqlCommand("select  id, nombre, apellidoPaterno, apellidoMaterno, sexo from veterinarios where id="+id.ToString(), conexion);
                using (var reader = query.ExecuteReader())
                {
                    // try
                    //{
                        reader.Read();
                    veterinario = new VeterinarioModel()
                    {
                            id = Convert.ToInt32(reader["Id"]),
                            nombre = reader["nombre"].ToString(),
                            apellidoPaterno = reader["apellidoPaterno"].ToString(),
                            apellidoMaterno = reader["apellidoMaterno"].ToString(),
                            sexo = reader["sexo"].ToString()
                        };

                   // }
                   /* catch (Exception e)
                    {
                        Console.WriteLine("ERRORSS"+e.Message);
                    } */

                }

                if (veterinario == null)
                {
                    return NotFound();

                }else
                {
                    return Ok(veterinario);
                }
            }
        }

        public IHttpActionResult GetVeterinarioByConcidence(string valor)
        {
            List<VeterinarioModel> lstVeterinarios = new List<VeterinarioModel>();

            using (MySqlConnection conexion = ConexionBase.GetDBConnection())
            {

                try
                {
                    MySqlCommand query = new MySqlCommand("buscarVeterinario", conexion);
                    query.CommandType = CommandType.StoredProcedure;

                    // query.CommandText = "buscarVeterinario";
                    query.Parameters.AddWithValue("@valor", valor);
                    query.Parameters["@valor"].Direction = ParameterDirection.Input;
                    conexion.Open();
                    // query.ExecuteNonQuery();
                    using (var reader = query.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lstVeterinarios.Add(new VeterinarioModel()
                            {

                                nombre = reader["nombre"].ToString(),
                                apellidoPaterno = reader["apellidoPaterno"].ToString(),
                                apellidoMaterno = reader["apellidoMaterno"].ToString(),
                                sexo = reader["sexo"].ToString()
                            });
                        }

                        conexion.Close(); 
                    }
                    
                }catch (Exception e)
                {
                    conexion.Close();
                    Console.WriteLine("Error al consultar los datos"+ e.Message);
                }


            }
            if (lstVeterinarios.Count == 0)
            {
                return NotFound();

            }
            else
            {
                return Ok(lstVeterinarios);
            }

        }


        public IHttpActionResult PostNewVeterinario(VeterinarioModel veterinario)
        {
            if (!ModelState.IsValid)
                return BadRequest("Datos invalidos");

            using ( MySqlConnection conexion = ConexionBase.GetDBConnection()){
                    conexion.Open();
                    string insert = "insert into veterinarios (id, nombre, apellidoPaterno, apellidoMaterno, sexo) values ('"+veterinario.id+"', '"+veterinario.nombre+ "', '"+veterinario.apellidoPaterno+ "', '" +veterinario.apellidoMaterno+ "', '" + veterinario.sexo +"' );";
                    MySqlCommand query = new MySqlCommand(insert, conexion);
                    MySqlDataReader myReader;
                    myReader = query.ExecuteReader();
                    conexion.Close();
                    return Ok();
                }
         }

        public IHttpActionResult Put(VeterinarioModel veterinario)
        {
            if (!ModelState.IsValid)
                return BadRequest("Modelo de datos invalido");

            using (MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                conexion.Open();
                string update = "update veterinarios set nombre='" + veterinario.nombre + "',apellidoPaterno='" + veterinario.apellidoPaterno + "',apellidoMaterno='" + veterinario.apellidoMaterno + "',sexo='" + veterinario.sexo + "'where id='" + veterinario.id + "';";
                MySqlCommand query = new MySqlCommand(update, conexion);
                MySqlDataReader myReader;
                myReader = query.ExecuteReader();
                conexion.Close();
                return Ok();

            }
        }

        public IHttpActionResult Delete(int id)
        {
            if (id <= 0)
                return BadRequest("Id no válido");

            using (MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                conexion.Open();
                string delete = "delete from veterinarios where id="+id.ToString();
                MySqlCommand query = new MySqlCommand(delete, conexion);
                MySqlDataReader myReader;
                myReader = query.ExecuteReader();
                conexion.Close();
                return Ok();
            }

           
        }
            


            
        
    }
}
