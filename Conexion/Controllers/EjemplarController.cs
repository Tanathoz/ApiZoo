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
    public class EjemplarController : ApiController
    {
        public IHttpActionResult GetAllEjemplares()
        {
            List<EjemplarViewModel> lstEjemplares = new List<EjemplarViewModel>();
            using (MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                conexion.Open();
                MySqlCommand query = new MySqlCommand("select * from ejemplares", conexion);
                using (var reader = query.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lstEjemplares.Add(new EjemplarViewModel()
                        {
                            marcaje = reader["marcaje"].ToString(),
                            idAnimal = Convert.ToInt32(reader["idAnimal"]),
                            fechaNacimiento = reader["fechaNacimiento"].ToString(),
                            fechaAlta = reader["fechaAlta"].ToString(),
                            sexo = reader["sexo"].ToString(),
                            nombre = reader["nombrePropio"].ToString()

                        });
                    }
                }
            }

            if (lstEjemplares.Count == 0)
            {
                return NotFound();
            }else
            {
                return Ok(lstEjemplares);
            }
        }

        public IHttpActionResult GetEjemplarByMarcaje (string marcaje)
        {
            EjemplarViewModel ejemplar = null;
            using (MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                conexion.Open();
                MySqlCommand query = new MySqlCommand("Select idAnimal, fechaNacimiento, fechaAlta, sexo, nombrePropio where marcaje=" + marcaje.ToString(), conexion);
                using (var reader = query.ExecuteReader())
                {
                    reader.Read();
                    ejemplar = new EjemplarViewModel()
                    {
                        idAnimal = Convert.ToInt32(reader["idAnimal"]),
                        fechaNacimiento = reader["fechaNacimiento"].ToString(),
                        fechaAlta = reader["fechaAlta"].ToString(),
                        sexo = reader["sexo"].ToString(),
                        nombre = reader["nombrePropio"].ToString()
                    };

                    if (ejemplar == null)
                    {
                        conexion.Close();
                        return NotFound();
                    }else
                    {
                        conexion.Close();
                        return Ok(ejemplar);
                    }
                }
            }
        }

        public IHttpActionResult GetEjemplarByCoincidence(string valor)
        {
            List<EjemplarViewModel> lstEjemplar = new List<EjemplarViewModel>();
            using (MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                MySqlCommand query = new MySqlCommand("buscarEjemplar", conexion);
                query.CommandType = CommandType.StoredProcedure;
                query.Parameters.AddWithValue("@valor", valor);
                query.Parameters["@valor"].Direction = ParameterDirection.Input;
                conexion.Open();
                using (var reader = query.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lstEjemplar.Add(new EjemplarViewModel()
                        {
                            marcaje = reader["marcaje"].ToString(),
                            idAnimal = Convert.ToInt32(reader["idAnimal"]),
                            fechaNacimiento = reader["fechaNaciminto"].ToString(),
                            fechaAlta = reader["fechaAlta"].ToString(),
                            sexo = reader["sexo"].ToString(),
                            nombre = reader["nombrePropio"].ToString()
                        });

                    }
                }

                conexion.Close();
                if (lstEjemplar.Count == 0)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(lstEjemplar);
                }
            }
        }

        public IHttpActionResult PostNewEjemplar(EjemplarViewModel ejemplar)
        {
            if (!ModelState.IsValid)
                return BadRequest("Datos invalidos");
            using (MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                conexion.Open();
                string insert = "insert into ejemplares (marcaje, idAnimal, fechaNacimiento, fechaAlta, sexo, nombrePropio) values ( '" + ejemplar.marcaje + "' , '" + ejemplar.idAnimal + "' , '" + ejemplar.fechaNacimiento + "',  '" + ejemplar.fechaAlta + "', '" + ejemplar.sexo + "' , '" + ejemplar.nombre + "'  );";
                MySqlCommand query = new MySqlCommand(insert, conexion);
                MySqlDataReader myReader;
                myReader = query.ExecuteReader();
                conexion.Close();
                return Ok();                
            }
        }

        public IHttpActionResult putAnimal(EjemplarViewModel ejemplar)
        {
            if (!ModelState.IsValid)
                return BadRequest("Modelo de datos invalido");
            using (MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                conexion.Open();
                string update = "update ejemplares set idAnimal= '" + ejemplar.idAnimal + "', fechaNacimiento='" + ejemplar.fechaNacimiento + "' , fechaAlta='" + ejemplar.sexo + "', nombrePropio='" + ejemplar.nombre + "' ;";
                MySqlCommand query = new MySqlCommand(update, conexion);
                MySqlDataReader myReader;
                myReader = query.ExecuteReader();
                conexion.Close();
                return Ok();
            }
        }

        public IHttpActionResult Delete(string  marcaje)
        {
            if (marcaje == null || marcaje ==string.Empty)
                return BadRequest("Marcaje no válido");
            using (MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                conexion.Open();
                string delete = "delete from ejemplares where marcaje=" + marcaje.ToString();
                MySqlCommand query = new MySqlCommand(delete, conexion);
                MySqlDataReader myReader;
                myReader = query.ExecuteReader();
                conexion.Close();
                return Ok();
            }
        }




    }
}
