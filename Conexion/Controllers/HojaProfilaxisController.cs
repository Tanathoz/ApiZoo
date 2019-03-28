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
    public class HojaProfilaxisController : ApiController
    {
        public IHttpActionResult GetAllHojas()
        {
            List<HojaProfilaxisViewModel> lstHojas = new List<HojaProfilaxisViewModel>();
            using (MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                conexion.Open();
                string consulta = "Select h.id, h.lugar, h.fecha, h.tratamiento,h.observaciones, h.comentarios, h.marcajeEjemplar,e.nombrePropio, a.nombreComun from hojaprofilaxis h inner join ejemplares e on h.marcajeEjemplar= e.marcaje inner join animal a on e.idAnimal=a.id;";
                MySqlCommand query = new MySqlCommand(consulta, conexion);
                using (var reader = query.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lstHojas.Add(new HojaProfilaxisViewModel
                        {
                            id = Convert.ToInt32(reader["id"].ToString()),
                            lugar = reader["lugar"].ToString(),
                            fecha = reader["fecha"].ToString(),
                            tratamiento = reader["tratamiento"].ToString(),
                            marcaje = reader["marcajeEjemplar"].ToString(),
                            nombrePropio = reader["nombrePropio"].ToString(),
                            nombreComun = reader["nombreComun"].ToString()
                        });
                    }
                }

                if (lstHojas.Count == 0)
                {
                    return NotFound();
                }else
                {
                    return Ok(lstHojas);
                }
            }

        }


        public IHttpActionResult GetHojaById(int id)
        {
            HojaProfilaxisViewModel hoja = null;
            using (MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                conexion.Open();
                string consulta = "Select h.id, h.lugar, DATE_FORMAT(h.fecha, '%Y-%m-%d' ) AS fecha ,h.tratamiento,h.observaciones , DATE_FORMAT(h.fechaAplicacion, '%Y-%m-%d') AS fechaAplicacion, DATE_FORMAT(h.fechaProxima, '%Y-%m-%d' ) As fechaProxima ,h.marcajeEjemplar,h.idVeterinario, e.nombrePropio, a.nombreComun from hojaprofilaxis h inner join ejemplares e on h.marcajeEjemplar = e.marcaje inner join animal a on e.idAnimal = a.id where h.id=" + id.ToString();
                MySqlCommand query = new MySqlCommand(consulta, conexion);
                using (var reader = query.ExecuteReader())
                {
                    reader.Read();
                    hoja = new HojaProfilaxisViewModel()
                    {
                        lugar = reader["lugar"].ToString(),
                        fecha = reader["fecha"].ToString(),
                        tratamiento = reader["tratamiento"].ToString(),
                        observaciones = reader["observaciones"].ToString(),
                        fechaAplicacion = reader["fechaAplicacion"].ToString(),
                        fechaProxima = reader["fechaProxima"].ToString(),
                        marcaje = reader["marcajeEjemplar"].ToString(),
                        nombrePropio = reader["nombrePropio"].ToString(),
                        nombreComun = reader["nombreComun"].ToString(),
                        idVeterinario = Convert.ToInt32(reader["idVeterinario"].ToString())
                    };
                }
            }

            if (hoja == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(hoja);
            }
        }

        public IHttpActionResult GetHojaByConcidence (string valor)
        {
            List<HojaProfilaxisViewModel> lstHojas = new List<HojaProfilaxisViewModel>();
            using (MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                MySqlCommand query = new MySqlCommand("consultaProfilaxisByConcidence", conexion);
                query.CommandType = CommandType.StoredProcedure;
                query.Parameters.AddWithValue("@valor", valor);
                query.Parameters["@valor"].Direction = ParameterDirection.Input;
                conexion.Open();
                using (var reader = query.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lstHojas.Add(new HojaProfilaxisViewModel
                        {
                            lugar = reader["lugar"].ToString(),
                            fecha = reader["fecha"].ToString(),
                            tratamiento = reader["tratamiento"].ToString(),
                            marcaje = reader["marcajeEjemplar"].ToString(),
                            nombrePropio = reader["nombrePropio"].ToString(),
                            nombreComun = reader["nombreComun"].ToString()
                        });

                        
                    }
                }
                if (lstHojas.Count == 0)
                {
                    return NotFound();
                }else
                {
                    return Ok(lstHojas);
                }

            }
        }

        public IHttpActionResult PostNewHoja(HojaProfilaxisViewModel hoja)
        {
            if (!ModelState.IsValid)
                return BadRequest("Modelo de datos inválido");
            using (MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                MySqlCommand query = new MySqlCommand("insertarProfilaxis", conexion);
                query.CommandType = CommandType.StoredProcedure;
                query.Parameters.AddWithValue("@id", hoja.id);
                query.Parameters.AddWithValue("@lugar", hoja.lugar);
                query.Parameters.AddWithValue("@fecha", hoja.fecha);
                query.Parameters.AddWithValue("@tratamiento", hoja.tratamiento);
                query.Parameters.AddWithValue("@fechaAplicacion", hoja.fechaAplicacion);
                query.Parameters.AddWithValue("@observaciones", hoja.observaciones);
                query.Parameters.AddWithValue("@fechaProxima", hoja.fechaProxima );
                query.Parameters.AddWithValue("@marcajeEjemplar", hoja.marcaje);
                query.Parameters.AddWithValue("@idVeterinario", hoja.idVeterinario);

                query.Parameters["@id"].Direction = ParameterDirection.Input;
                query.Parameters["@lugar"].Direction = ParameterDirection.Input;
                query.Parameters["@fecha"].Direction = ParameterDirection.Input;
                query.Parameters["@tratamiento"].Direction = ParameterDirection.Input;
                query.Parameters["@fechaAplicacion"].Direction = ParameterDirection.Input;
                query.Parameters["@observaciones"].Direction = ParameterDirection.Input;
                query.Parameters["@fechaProxima"].Direction = ParameterDirection.Input;
                query.Parameters["@marcajeEjemplar"].Direction = ParameterDirection.Input;
                query.Parameters["@idVeterinario"].Direction = ParameterDirection.Input;
                conexion.Open();
                MySqlDataReader reader;
                reader = query.ExecuteReader();
                conexion.Close();
                return Ok();
            }
        }

        [HttpPut]
        public IHttpActionResult PutHojaProfilaxis (HojaProfilaxisViewModel hoja)
        {
            if (!ModelState.IsValid)
                return BadRequest("Modelo de datos hoja de profilaxis invalido");
            using (MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                MySqlCommand query = new MySqlCommand("editarProfilaxis", conexion);
                query.CommandType = CommandType.StoredProcedure;
                query.Parameters.AddWithValue("@idHoja", hoja.id);
                query.Parameters.AddWithValue("@lugar", hoja.lugar);
                query.Parameters.AddWithValue("@fecha", hoja.fecha);
                query.Parameters.AddWithValue("@tratamiento", hoja.tratamiento);
                query.Parameters.AddWithValue("@fechaAplicacion", hoja.fechaAplicacion);
                query.Parameters.AddWithValue("@observaciones", hoja.observaciones);
                query.Parameters.AddWithValue("@fechaProxima", hoja.fechaProxima);
                query.Parameters.AddWithValue("@marcajeEjemplar", hoja.marcaje);
                query.Parameters.AddWithValue("@idVeterinario", hoja.idVeterinario);

                query.Parameters["@idHoja"].Direction = ParameterDirection.Input;
                query.Parameters["@lugar"].Direction = ParameterDirection.Input;
                query.Parameters["@fecha"].Direction = ParameterDirection.Input;
                query.Parameters["@tratamiento"].Direction = ParameterDirection.Input;
                query.Parameters["@fechaAplicacion"].Direction = ParameterDirection.Input;
                query.Parameters["@observaciones"].Direction = ParameterDirection.Input;
                query.Parameters["@fechaProxima"].Direction = ParameterDirection.Input;
                query.Parameters["@marcajeEjemplar"].Direction = ParameterDirection.Input;
                query.Parameters["@idVeterinario"].Direction = ParameterDirection.Input;
                conexion.Open();
                MySqlDataReader reader;
                reader = query.ExecuteReader();
                conexion.Close();
                return Ok();
            }

        }


        [HttpDelete]
        public IHttpActionResult delete (int id)
        {
            if (id <= 0)
                return BadRequest("Id de hoja profilaxis inválido");
            using (MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                MySqlCommand query = new MySqlCommand("borrarHojaProfilax", conexion);
                query.CommandType = CommandType.StoredProcedure;
                query.Parameters.AddWithValue("@idHoja", id);
                query.Parameters["@idHoja"].Direction = ParameterDirection.Input;
                conexion.Open();
                MySqlDataReader reader = query.ExecuteReader();
                conexion.Close();
                return Ok();

            }
        }

    }
}
