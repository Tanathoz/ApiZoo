using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MySql.Data.MySqlClient;
using Conexion.Models;
using System.Data;

namespace Conexion.Controllers
{
    public class AnimalController : ApiController
    {
        public IHttpActionResult GetAllAnimales()
        {
            List<AnimalViewModel> lstAnimales = new List<AnimalViewModel>();
            using (MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                conexion.Open();
                MySqlCommand query = new MySqlCommand("select * from animal", conexion);
                using (var reader = query.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lstAnimales.Add(new AnimalViewModel()
                        {
                            id = Convert.ToInt32(reader["id"]),
                            nombreCientifico = reader["nombreCientifico"].ToString(),
                            nombreComun = reader["nombreComun"].ToString(),
                            familia = reader["familia"].ToString(),
                            clase = reader["clase"].ToString(),
                            orden = reader["orden"].ToString(),
                            especie = reader["especie"].ToString(),
                            habitat = reader["habitat"].ToString(),
                            gestacion = reader["gestacion"].ToString(),
                            camada = reader["camada"].ToString(),
                            longevidad = reader["longevidad"].ToString(),
                            peso = reader["peso"].ToString(),
                            ubicacionGeografica = reader["ubicacionGeografica"].ToString(),
                            alimentacion = reader["Alimentacion"].ToString()

                        });
                    }
                }
            }

            if (lstAnimales.Count == 0)
            {
                return NotFound();
            }
            else
            {
                return Ok(lstAnimales);
            }
        }

        public IHttpActionResult GetAnimalById(int id)
        {
            AnimalViewModel animal = null;
            using (MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                conexion.Open();
                MySqlCommand query = new MySqlCommand("select nombreCientifico, nombreComun, familia, clase, orden, especie, habitat, gestacion, camada, longevidad, peso, ubicacionGeografica, Alimentacion from animal where id=" + id.ToString(), conexion);
                using (var reader = query.ExecuteReader())
                {
                    reader.Read();
                    animal = new AnimalViewModel()
                    {
                        id = Convert.ToInt32(reader["id"]),
                        nombreCientifico = reader["nombreCientifico"].ToString(),
                        nombreComun = reader["nombreComun"].ToString(),
                        familia = reader["familia"].ToString(),
                        clase = reader["clase"].ToString(),
                        orden = reader["orden"].ToString(),
                        especie = reader["especie"].ToString(),
                        habitat = reader["habitat"].ToString(),
                        gestacion = reader["gestacion"].ToString(),
                        camada = reader["camada"].ToString(),
                        longevidad = reader["longevidad"].ToString(),
                        peso = reader["peso"].ToString(),
                        ubicacionGeografica = reader["ubicacionGeografica"].ToString(),
                        alimentacion = reader["Alimentacion"].ToString()
                    };
                    if (animal == null)
                    {
                        conexion.Close();
                        return NotFound();
                    }
                    else
                    {
                        conexion.Close();
                        return Ok(animal);
                    }

                }
            }

        }

        public IHttpActionResult GetAnimalByConcidence(string valor)
        {
            List<AnimalViewModel> lstAnimales = new List<AnimalViewModel>();
            using (MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                try
                {
                    MySqlCommand query = new MySqlCommand("buscarAnimal", conexion);
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
                            lstAnimales.Add(new AnimalViewModel()
                            {
                                
                                nombreCientifico = reader["nombreCientifico"].ToString(),
                                nombreComun = reader["nombreComun"].ToString(),
                                familia = reader["familia"].ToString(),
                                clase = reader["clase"].ToString(),
                                orden = reader["orden"].ToString(),
                                especie = reader["especie"].ToString(),
                                habitat = reader["habitat"].ToString()
                                

                            });
                        }

                        conexion.Close();

                    }
                }
                catch (Exception e)
                {
                    conexion.Close();
                    Console.WriteLine("Error al consultar los datos" + e.Message);
                }
            }


            if (lstAnimales.Count() == 0)
            {
                return NotFound();
            }else
            {
                return Ok(lstAnimales);
            }
        }

        public IHttpActionResult PostNewAnimal(AnimalViewModel animal)
        {
            if (!ModelState.IsValid)
                return BadRequest("Datos invalidos");

            using (MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                conexion.Open();
                string insert = "insert into veterinarios values ('" + animal.id + "', '" + animal.nombreCientifico + "', '" + animal.nombreComun + "', '" + animal.familia + "', '" + animal.clase + "', '" + animal.orden + "', '" + animal.especie + "', '" + animal.habitat + "', '" + animal.gestacion + "', '" + animal.camada + "', '" + animal.longevidad + "', '" + animal.peso + "', '" + animal.ubicacionGeografica + "', '" + animal.alimentacion + "' );";
                MySqlCommand query = new MySqlCommand(insert, conexion);
                MySqlDataReader myReader;
                myReader = query.ExecuteReader();
                conexion.Close();
                return Ok();
            }
        }

        public IHttpActionResult putAnimal(AnimalViewModel animal)
        {
            if (!ModelState.IsValid)
                return BadRequest("Modelo de datos invalido");
            using (MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                conexion.Open();
                string update = "update veterinarios set nombreCientifico='" + animal.nombreCientifico + "',nombreComun='" + animal.nombreComun + "',familia='" + animal.familia + "',clase='" + animal.clase + "',orden='" + animal.orden + "',especie='" + animal.especie + "', habitat='" + animal.habitat + "',gestacion='" + animal.gestacion + "',camada='" + animal.camada + "',longevidad='" + animal.longevidad + "',peso='" + animal.peso + "',ubicacionGeografica='" + animal.ubicacionGeografica + "',Alimentacion='" + animal.alimentacion + "'where id='" + animal.id + "';";
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
                string delete = "delete from animal where id=" + id.ToString();
                MySqlCommand query = new MySqlCommand(delete, conexion);
                MySqlDataReader myReader;
                myReader = query.ExecuteReader();
                conexion.Close();
                return Ok();
            }
        }
    }
}
