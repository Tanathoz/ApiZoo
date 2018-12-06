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
                MySqlCommand query = new MySqlCommand("select e.marcaje, e.idAnimal, a.nombreComun, e.fechaNacimiento, e.fechaAlta, e.sexo , e.nombrePropio from ejemplares e inner join animal a on e.idAnimal= a.id", conexion);
                using (var reader = query.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lstEjemplares.Add(new EjemplarViewModel()
                        {
                            marcaje = reader["marcaje"].ToString(),
                            idAnimal = Convert.ToInt32(reader["idAnimal"]),
                            nombreComun = reader["nombreComun"].ToString(),
                            fechaNacimiento = reader["fechaNacimiento"].ToString(),
                            fechaAlta = reader["fechaAlta"].ToString(),
                            sexo = reader["sexo"].ToString(),
                            nombrePropio = reader["nombrePropio"].ToString()

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
               
                MySqlCommand query = new MySqlCommand("consultarEjemplar", conexion);
                query.CommandType = CommandType.StoredProcedure;
                query.Parameters.AddWithValue("@valor", marcaje);
                query.Parameters["@valor"].Direction = ParameterDirection.Input;
                conexion.Open();

                using (var reader = query.ExecuteReader())
                {
                    reader.Read();
                    ejemplar = new EjemplarViewModel()
                    {
                        idAnimal = Convert.ToInt32(reader["idAnimal"]),
                        nombreComun = reader["nombreComun"].ToString(),
                        fechaNacimiento = reader["fechaNacimiento"].ToString(),
                        fechaAlta = reader["fechaAlta"].ToString(),
                        sexo = reader["sexo"].ToString(),
                        nombrePropio = reader["nombrePropio"].ToString()
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
                            nombreComun = reader["nombreComun"].ToString(),
                            fechaNacimiento = reader["fechaNacimiento"].ToString(),
                            fechaAlta = reader["fechaAlta"].ToString(),
                            sexo = reader["sexo"].ToString(),
                            nombrePropio = reader["nombrePropio"].ToString()
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
                string insert = "insert into ejemplares (marcaje, idAnimal, fechaNacimiento, fechaAlta, sexo, nombrePropio) values ( '" + ejemplar.marcaje + "' , '" + ejemplar.idAnimal + "' , '" + ejemplar.fechaNacimiento + "',  '" + ejemplar.fechaAlta + "', '" + ejemplar.sexo + "' , '" + ejemplar.nombrePropio + "'  );";
                MySqlCommand query = new MySqlCommand(insert, conexion);
                MySqlDataReader myReader;
                myReader = query.ExecuteReader();
                conexion.Close();
                return Ok();                
            }
        }

        public IHttpActionResult putEjemplar(EjemplarViewModel ejemplar)
        {
            if (!ModelState.IsValid)
                return BadRequest("Modelo de datos invalido");
            using (MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                conexion.Open();
                string update = "update ejemplares set idAnimal=" + ejemplar.idAnimal + ",fechaNacimiento='" + ejemplar.fechaNacimiento + "' ,fechaAlta='" + ejemplar.fechaAlta + "',sexo='" + ejemplar.sexo + "',nombrePropio='" + ejemplar.nombrePropio + "' where marcaje='" + ejemplar.marcaje + "';";
                MySqlCommand query = new MySqlCommand(update, conexion);
                MySqlDataReader myReader;
                myReader = query.ExecuteReader();
                conexion.Close();
                return Ok();
            }
        }

        public IHttpActionResult Delete(string  marcajeDelete)
        {
            if (marcajeDelete == null || marcajeDelete ==string.Empty)
                return BadRequest("Marcaje no válido");

            using (MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                conexion.Open();
                string delete = "delete from ejemplares where marcaje='" + marcajeDelete.ToString()+"';";
                MySqlCommand query = new MySqlCommand(delete, conexion);
                MySqlDataReader myReader;
                myReader = query.ExecuteReader();
                conexion.Close();
                return Ok();
            }
        }




    }
}
