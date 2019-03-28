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
    public class NecropsiaController : ApiController
    {

        public IHttpActionResult GetAllNecropsias()
        {
            List<NecropsiaViewModel> lstNecropsias = new List<NecropsiaViewModel>();
            using (MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                conexion.Open();
                string consulta = "Select n.id, n.lugar, n.marcajeEjemplar, n.diagnosticoMuerte, e.nombrePropio, a.nombreComun from necropsias n inner join ejemplares e on n.marcajeEjemplar= e.marcaje inner join animal a on e.idAnimal = a.id;";
                MySqlCommand query = new MySqlCommand(consulta, conexion);
                using (var reader = query.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lstNecropsias.Add(new NecropsiaViewModel
                        {
                            id = Convert.ToInt32(reader["id"].ToString()),
                            lugar = reader["lugar"].ToString(),
                            diagnosticoMuerte = reader["diagnosticoMuerte"].ToString(),
                            marcaje = reader["marcajeEjemplar"].ToString(),
                            nombrePropio = reader["nombrePropio"].ToString(),
                            nombreComun = reader["nombreComun"].ToString(),

                        });
                    }
                }

                if(lstNecropsias.Count == 0)
                {
                    return NotFound();
                }else
                {
                    return Ok(lstNecropsias);
                }
            }

        }

        public IHttpActionResult GetNecropsiaById(int id)
        {
            NecropsiaViewModel hoja = null;
            using (MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                conexion.Open();
                string consulta = "Select n.id, n.lugar, DATE_FORMAT(n.fecha, '%Y-%m-%d' ) AS fecha ,n.hora, n.antecedentes, n.estadoFisico, n.idVeterinario, n.idEncargado , n.lesiones , n.toracica,n.abdominal, n.muestras , n.marcajeEjemplar, n.diagnosticoMuerte, e.nombrePropio, a.nombreComun from necropsias n inner join ejemplares e on n.marcajeEjemplar= e.marcaje inner join animal a on e.idAnimal = a.id where n.id="+id;
                MySqlCommand query = new MySqlCommand(consulta, conexion);
                using (var reader = query.ExecuteReader())
                {
                    reader.Read();
                    hoja = new NecropsiaViewModel()
                    {
                        lugar = reader["lugar"].ToString(),
                        fecha = reader["fecha"].ToString(),
                        hora = reader["hora"].ToString(),
                        antecedentes = reader["antecedentes"].ToString(),
                        diagnosticoMuerte = reader["diagnosticoMuerte"].ToString(),
                        estadoFisico = reader["estadoFisico"].ToString(),
                        lesiones = reader["lesiones"].ToString(),
                        toracica = reader["toracica"].ToString(),
                        abdominal = reader["abdominal"].ToString(),
                        muestras = reader["muestras"].ToString(),
                        marcaje = reader["marcajeEjemplar"].ToString(),
                        nombrePropio = reader["nombrePropio"].ToString(),
                        nombreComun = reader["nombreComun"].ToString(),
                        idVeterinario = Convert.ToInt32(reader["idVeterinario"].ToString()),
                        idEncargado = Convert.ToInt32(reader["idEncargado"].ToString())
                    };
                }
            }

            if (hoja == null)
            {
                return NotFound();
            }else
            {
                return Ok(hoja);
            }
        }


        public IHttpActionResult GetHojaByConcidence(string valor)
        {
            List<NecropsiaViewModel> lstNecropsia = new List<NecropsiaViewModel>();
            using (MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                MySqlCommand query = new MySqlCommand("consultaNecropsiaByConcidence", conexion);
                query.CommandType = CommandType.StoredProcedure;
                query.Parameters.AddWithValue("@valor", valor);
                query.Parameters["@valor"].Direction = ParameterDirection.Input;
                conexion.Open();
                using (var reader = query.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lstNecropsia.Add(new NecropsiaViewModel
                        {
                            diagnosticoMuerte = reader["diagnosticoMuerte"].ToString(),
                            marcaje = reader["marcaje"].ToString(),
                            nombrePropio = reader["nombrePropio"].ToString(),
                            nombreComun = reader["nombreComun"].ToString()
                        });
                    }
                }
            }

            if (lstNecropsia.Count == 0)
            {
                return NotFound();
            }else
            {
                return Ok(lstNecropsia);
            }
        }

        public IHttpActionResult PostNewNecropsia(NecropsiaViewModel necropsia)
        {
            if (!ModelState.IsValid)
                return BadRequest("Modelo de datos invalido ");
            using (MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                MySqlCommand query = new MySqlCommand("registroNecropsia", conexion);
                query.CommandType = CommandType.StoredProcedure;
                query.Parameters.AddWithValue("@id", necropsia.id);
                query.Parameters.AddWithValue("@lugar", necropsia.lugar);
                query.Parameters.AddWithValue("@fecha", necropsia.fecha);
                query.Parameters.AddWithValue("@marcajeEjemplar", necropsia.marcaje);
                query.Parameters.AddWithValue("@hora", necropsia.hora);
                query.Parameters.AddWithValue("@antecedentes", necropsia.antecedentes);
                query.Parameters.AddWithValue("@diagnosticoMuerte", necropsia.diagnosticoMuerte);
                query.Parameters.AddWithValue("@estadoFisico", necropsia.estadoFisico);
                query.Parameters.AddWithValue("@lesiones", necropsia.lesiones);
                query.Parameters.AddWithValue("@toracica", necropsia.toracica);
                query.Parameters.AddWithValue("@abdominal", necropsia.abdominal);
                query.Parameters.AddWithValue("@muestras", necropsia.muestras);
                query.Parameters.AddWithValue("@idVeterinario", necropsia.idVeterinario);
                query.Parameters.AddWithValue("@idEncargado",necropsia.idEncargado );
                query.Parameters["@id"].Direction = ParameterDirection.Input;
                query.Parameters["@lugar"].Direction = ParameterDirection.Input;
                query.Parameters["@fecha"].Direction = ParameterDirection.Input;
                query.Parameters["@marcajeEjemplar"].Direction = ParameterDirection.Input;
                query.Parameters["@hora"].Direction = ParameterDirection.Input;
                query.Parameters["@antecedentes"].Direction = ParameterDirection.Input;
                query.Parameters["@diagnosticoMuerte"].Direction = ParameterDirection.Input;
                query.Parameters["@estadoFisico"].Direction = ParameterDirection.Input;
                query.Parameters["@lesiones"].Direction = ParameterDirection.Input;
                query.Parameters["@toracica"].Direction = ParameterDirection.Input;
                query.Parameters["@abdominal"].Direction = ParameterDirection.Input;
                query.Parameters["@muestras"].Direction = ParameterDirection.Input;
                query.Parameters["@idVeterinario"].Direction = ParameterDirection.Input;
                query.Parameters["@idEncargado"].Direction = ParameterDirection.Input;

                conexion.Open();
                MySqlDataReader reader;
                reader = query.ExecuteReader();
                conexion.Close();
                return Ok();

            }
        }

        [HttpPut]
        public IHttpActionResult PutNecropsia (NecropsiaViewModel necropsia)
        {
            if (!ModelState.IsValid)
                return BadRequest("Modelo de datos de necropsia invalido");
            using (MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                MySqlCommand query = new MySqlCommand("editarNecropsia", conexion);
               
                query.CommandType = CommandType.StoredProcedure;
                query.Parameters.AddWithValue("@idNecropsia", necropsia.id);
                query.Parameters.AddWithValue("@lugar", necropsia.lugar);
                query.Parameters.AddWithValue("@fecha", necropsia.fecha);
                query.Parameters.AddWithValue("@marcajeEjemplar", necropsia.marcaje);
                query.Parameters.AddWithValue("@hora", necropsia.hora);
                query.Parameters.AddWithValue("@antecedentes", necropsia.antecedentes);
                query.Parameters.AddWithValue("@diagnosticoMuerte", necropsia.diagnosticoMuerte);
                query.Parameters.AddWithValue("@estadoFisico", necropsia.estadoFisico);
                query.Parameters.AddWithValue("@lesiones", necropsia.lesiones);
                query.Parameters.AddWithValue("@toracica", necropsia.toracica);
                query.Parameters.AddWithValue("@abdominal", necropsia.abdominal);
                query.Parameters.AddWithValue("@muestras", necropsia.muestras);
                query.Parameters.AddWithValue("@idVeterinario", necropsia.idVeterinario);
                query.Parameters.AddWithValue("@idEncargado", necropsia.idEncargado);
                query.Parameters["@idNecropsia"].Direction = ParameterDirection.Input;
                query.Parameters["@lugar"].Direction = ParameterDirection.Input;
                query.Parameters["@fecha"].Direction = ParameterDirection.Input;
                query.Parameters["@marcajeEjemplar"].Direction = ParameterDirection.Input;
                query.Parameters["@hora"].Direction = ParameterDirection.Input;
                query.Parameters["@antecedentes"].Direction = ParameterDirection.Input;
                query.Parameters["@diagnosticoMuerte"].Direction = ParameterDirection.Input;
                query.Parameters["@estadoFisico"].Direction = ParameterDirection.Input;
                query.Parameters["@lesiones"].Direction = ParameterDirection.Input;
                query.Parameters["@toracica"].Direction = ParameterDirection.Input;
                query.Parameters["@abdominal"].Direction = ParameterDirection.Input;
                query.Parameters["@muestras"].Direction = ParameterDirection.Input;
                query.Parameters["@idVeterinario"].Direction = ParameterDirection.Input;
                query.Parameters["@idEncargado"].Direction = ParameterDirection.Input;

                conexion.Open();
                MySqlDataReader reader;
                reader = query.ExecuteReader();
                conexion.Close();
                return Ok();
            }
        }

        public IHttpActionResult delete (int id)
        {
            if (id <= 0)
                return BadRequest("Id de la necropsia invalido");
            using (MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                MySqlCommand query = new MySqlCommand("borrarNecropsia", conexion);
                query.CommandType = CommandType.StoredProcedure;
                query.CommandType = CommandType.StoredProcedure;
                query.Parameters.AddWithValue("@idNecropsia", id);
                query.Parameters["@idNecropsia"].Direction = ParameterDirection.Input;
                conexion.Open();
                MySqlDataReader readder = query.ExecuteReader();
                conexion.Close();
                return Ok();
            }
        }

    }
}
