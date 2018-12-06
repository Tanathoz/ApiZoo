using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using MySql.Data.MySqlClient;
using Conexion.Models;

namespace Conexion.Controllers
{
    public class DietaEjemplarController : ApiController
    {
        public IHttpActionResult GetAllDietasEj()
        {
            List<DietaEjemplarViewModel> lstDietas = new List<DietaEjemplarViewModel>();
            using (MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                
                MySqlCommand query = new MySqlCommand("consultaDietasEjemplares",conexion);
                query.CommandType = CommandType.StoredProcedure;
                conexion.Open();
                using (var reader = query.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lstDietas.Add(new DietaEjemplarViewModel()
                        {
                            marcaje = reader["marcajeEjemplar"].ToString(),
                            nombreComun = reader["nombreComun"].ToString(),
                            nombrePropio = reader["nombrePropio"].ToString(),
                            fechaCambio = reader["fechaCambio"].ToString(),
                            causaCambio = reader["causaCambio"].ToString(),
                            cantidad = reader["cantidad"].ToString(),
                            alimento = reader["alimento"].ToString(),
                            horario = reader["horario"].ToString(),
                            consideraciones = reader["consideraciones"].ToString()
                        });
                    }
                }

                conexion.Close();
                if (lstDietas.Count == 0)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(lstDietas);
                }
            }
        }

        public IHttpActionResult getDietaByMarcaje(string marcaje)
        {
            DietaEjemplarViewModel dieta = null;

            using (MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                conexion.Open();
                MySqlCommand query = new MySqlCommand("SELECT d.marcajeEjemplar, a.nombreComun, e.nombrePropio,d.fechaCambio, d.causaCambio, d.cantidad, d.alimento, d.horario, d.consideraciones from dietaejemplar d inner join ejemplares e on d.marcajeEjemplar = e.marcaje INNER JOIN animal a on e.idAnimal=a.id  where d.marcajeEjemplar="+marcaje,conexion);
                using (var reader = query.ExecuteReader())
                {
                    reader.Read();
                    dieta = new DietaEjemplarViewModel()
                    {
                        marcaje = reader["marcajeEjemplar"].ToString(),
                        nombreComun = reader["nombreComun"].ToString(),
                        nombrePropio = reader["nombrePropio"].ToString(),
                        fechaCambio = reader["fechaCambio"].ToString(),
                        causaCambio = reader["causaCambio"].ToString(),
                        cantidad = reader["cantidad"].ToString(),
                        alimento = reader["alimento"].ToString(),
                        horario = reader["horario"].ToString(),
                        consideraciones = reader["consideraciones"].ToString()
                    };

                }

                if (dieta == null)
                {
                    return NotFound();
                }else
                {
                    return Ok(dieta);
                }
            }
        }


        public IHttpActionResult getDietaByConcidence(string valor)
        {
            List<DietaEjemplarViewModel> lstDietas = new List<DietaEjemplarViewModel>();
            using (MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                MySqlCommand query = new MySqlCommand("buscarDietaEjemplar", conexion);
                query.CommandType = CommandType.StoredProcedure;
                query.Parameters.AddWithValue("@valor", valor);
                query.Parameters["@valor"].Direction = ParameterDirection.Input;
                conexion.Open();
                using (var reader = query.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lstDietas.Add(new DietaEjemplarViewModel()
                        {
                            marcaje = reader["marcajeEjemplar"].ToString(),
                            nombreComun = reader["nombreComun"].ToString(),
                            nombrePropio = reader["nombrePropio"].ToString(),
                            fechaCambio = reader["fechaCambio"].ToString(),
                            causaCambio = reader["causaCambio"].ToString(),
                            cantidad = reader["cantidad"].ToString(),
                            alimento = reader["alimento"].ToString(),
                            horario = reader["horario"].ToString(),
                            consideraciones = reader["consideraciones"].ToString()

                        });
                        
                       
                    }
                }
                conexion.Close();
                if (lstDietas.Count == 0)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(lstDietas);
                }
            } 
        }

        public IHttpActionResult PostNewDieta(DietaEjemplarViewModel dieta)
        {
            if (!ModelState.IsValid)
                return BadRequest("El modelo de datos es invalido");
            using (MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                MySqlCommand query = new MySqlCommand("insertarDietaEjemplar", conexion);
                query.CommandType = CommandType.StoredProcedure;
                query.Parameters.AddWithValue("@marcaje", dieta.marcaje);
                query.Parameters.AddWithValue("@fechaCambio", dieta.fechaCambio);
                query.Parameters.AddWithValue("@causaCambio", dieta.causaCambio);
                query.Parameters.AddWithValue("@cantidad", dieta.cantidad);
                query.Parameters.AddWithValue("@alimento", dieta.alimento);
                query.Parameters.AddWithValue("@horario", dieta.horario);
                query.Parameters.AddWithValue("@consideraciones", dieta.consideraciones);
                query.Parameters["@marcaje"].Direction = ParameterDirection.Input;
                query.Parameters["@fechaCambio"].Direction = ParameterDirection.Input;
                query.Parameters["@causaCambio"].Direction = ParameterDirection.Input;
                query.Parameters["@cantidad"].Direction = ParameterDirection.Input;
                query.Parameters["@alimento"].Direction = ParameterDirection.Input;
                query.Parameters["@horario"].Direction = ParameterDirection.Input;
                query.Parameters["@consideraciones"].Direction = ParameterDirection.Input;

                conexion.Open();
                MySqlDataReader myReader;
                myReader = query.ExecuteReader();
                conexion.Close();
                return Ok();

            }
        }


        public IHttpActionResult PutDieta(DietaEjemplarViewModel dieta)
        {
            if (!ModelState.IsValid)
                return BadRequest("Modelo de datos invalido");
            using (MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                conexion.Open();
                string update = "update dietaejemplar set fechaCambio='"+dieta.fechaCambio+ "', causaCambio='"+ dieta.causaCambio +"',cantidad='"+dieta.cantidad+"',alimento='"+dieta.alimento+ "', horario='"+dieta.horario+ "',consideraciones='"+ dieta.consideraciones+ "'where marcajeEjemplar='"+dieta.marcaje+"';"   ;
                MySqlCommand query = new MySqlCommand(update, conexion);
                MySqlDataReader myReader;
                myReader = query.ExecuteReader();
                conexion.Close();
                return Ok();
            }
        }

        public IHttpActionResult Delete(string marcaje)
        {
            if (marcaje == null || marcaje == string.Empty)
                return BadRequest("Id no válido error");
            using (MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                conexion.Open();
                string delete =" delete from dietaejemplar where marcajeEjemplar="+marcaje.ToString();
                MySqlCommand query = new MySqlCommand(delete, conexion);
                MySqlDataReader reader;
                reader = query.ExecuteReader();
                conexion.Close();
                return Ok();
            }
        }



    }
}
