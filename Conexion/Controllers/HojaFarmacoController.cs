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
    public class HojaFarmacoController : ApiController
    {


        public IHttpActionResult GetMaxIdHoja()
        {
            FarmacoClinica farmaco = null;
            using (MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                conexion.Open();
                string consulta = "Select id from hojaclinicas order by id desc limit 1";
                MySqlCommand query = new MySqlCommand(consulta, conexion);
                using (var reader = query.ExecuteReader())
                {
                    reader.Read();
                    farmaco = new FarmacoClinica()
                    {
                        idClinica = Convert.ToInt32(reader["id"].ToString())
                    };

                }
            }
            if (farmaco.idClinica == 0)
            {
                return NotFound();
            }
            else
            {
                return Ok(farmaco);
            }
        }

        public IHttpActionResult GetAllHojaFarmacos(int idHoja)
        {
            //int idHoja = 19;

            List<FarmacoClinica> lstFarmacos = new List<FarmacoClinica>();
            using (MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                conexion.Open();
                MySqlCommand query = new MySqlCommand("Select f.id,  f.nombre, f.via, fc.dosis, fc.frecuencia, DATE_FORMAT(fc.fechaAplicacion, '%Y-%m-%d' ) AS fechaAplicacion from  farmaco_clinicas fc inner join farmacos f on fc.idFarmaco=f.id where idClinica=" + idHoja.ToString(), conexion);
                using (var reader = query.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lstFarmacos.Add(new FarmacoClinica()
                        {
                            idFarmaco = Convert.ToInt32(reader["id"].ToString()),
                            nombreFarmaco = reader["nombre"].ToString(),
                            via = reader["via"].ToString(),
                            dosis = reader["dosis"].ToString(),
                            frecuencia = reader["frecuencia"].ToString(),
                            fechaAplicacion = reader["fechaAplicacion"].ToString()

                        });
                    }
                }
            }
            if (lstFarmacos.Count == 0)
                return NotFound();
            else
                return Ok(lstFarmacos);
        }

        public IHttpActionResult numeroFarmacos(int Lahoja)
        {
            int numFarmacos = 0;
            using (MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                conexion.Open();
                MySqlCommand query = new MySqlCommand("select  COUNT(*) as numFarmacos from farmaco_clinicas where idClinica=" + Lahoja, conexion);
                using (var reader = query.ExecuteReader())
                {
                    reader.Read();
                    numFarmacos = Convert.ToInt32(reader["numFarmacos"].ToString());

                }
            }

            if (numFarmacos > 0)
                return Ok(numFarmacos);
            else
                return NotFound();

        }
        [HttpPost]
        public IHttpActionResult PostNewHojaFarmaco(FarmacoClinica farmaco, string data)
        {
            if (!ModelState.IsValid)
                return BadRequest("Modelo de datos nos válido");
            using (MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                conexion.Open();
                string insert = "insert into farmaco_clinicas (idClinica, idFarmaco, dosis,frecuencia, fechaAplicacion) values (" + farmaco.idClinica + ", " + farmaco.idFarmaco + ", '" + farmaco.dosis + "', '" + farmaco.frecuencia + "', '" + farmaco.fechaAplicacion + "' );";
                MySqlCommand query = new MySqlCommand(insert, conexion);
                MySqlDataReader reader;
                reader = query.ExecuteReader();
                conexion.Close();
                return Ok();
            }
        }

       [HttpPut]
       public IHttpActionResult PutFarmaco(FarmacoClinica farmacoOtro)
        {
            if (!ModelState.IsValid)
                return BadRequest("Modelo de datos no válido");
            using (MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                conexion.Open();
                string update = "update farmaco_clinica set dosis='" + farmacoOtro.dosis + "', frecuencia='" + farmacoOtro.frecuencia + "', fechaAplicacion='" + farmacoOtro.fechaAplicacion + "' where idClinica=" + farmacoOtro.idClinica + " and idFarmaco=" + farmacoOtro.idFarmaco + ";";
                MySqlCommand query = new MySqlCommand(update, conexion);
                MySqlDataReader reader;
                reader = query.ExecuteReader();
                conexion.Close();
                return Ok();
            }

        }

        public IHttpActionResult Delete(int idFarmaco, int idHoja)
        {
            if (idFarmaco <= 0 || idHoja <= 0)
                return BadRequest("ID no válido");
            using (MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                conexion.Open();
                string delete = "delete from farmaco_clinicas where idClinica=" + idHoja + " and idFarmaco=" + idFarmaco;
                MySqlCommand query = new MySqlCommand(delete, conexion);
                MySqlDataReader reader;
                reader = query.ExecuteReader();
                conexion.Close();
                return Ok();

            }
        }

        public IHttpActionResult DeleteAllFarmacos(int idHoja)
        {
            if (idHoja <= 0)
                return BadRequest("Id no válido");
            using (MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                conexion.Open();
                string delete = "delete from farmaco_clinicas where idClinica=" + idHoja;
                MySqlCommand query = new MySqlCommand(delete, conexion);
                MySqlDataReader reader;
                reader = query.ExecuteReader();
                conexion.Close();
                return Ok();

            }
        }
    }
}
