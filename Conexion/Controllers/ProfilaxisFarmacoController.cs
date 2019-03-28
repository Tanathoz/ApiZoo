using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MySql.Data.MySqlClient;
using Conexion.Models;
using System.Web.Http.Cors;
using System.Threading.Tasks;

namespace Conexion.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ProfilaxisFarmacoController : ApiController
    {

        public IHttpActionResult GetMaxIdHoja()
        {
            FarmacoClinica farmaco = null;
            using (MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                conexion.Open();
                string consulta = "Select id from hojaprofilaxis order by id desc limit 1";
                MySqlCommand query = new MySqlCommand(consulta, conexion);
                using (var reader = query.ExecuteReader())
                {
                    reader.Read();
                    farmaco = new FarmacoClinica
                    {
                        idClinica = Convert.ToInt32(reader["id"].ToString())
                    };
                }
            }
            if (farmaco.idClinica == 0)
            {
                return NotFound();
            }else
            {
                return Ok(farmaco);
            }
        }

        public IHttpActionResult GetAllHojaProfilaxis (int idHoja)
        {
            List<FarmacoClinica> lstFarmacos = new List<FarmacoClinica>();
            using (MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                conexion.Open();
                MySqlCommand query = new MySqlCommand("Select f.id, f.nombre, f.via, fp.dosis, fp.frecuencia, DATE_FORMAT(fp.fechaAplicacion, '%Y-%m-%d' ) AS fechaAplicacion from farmacoprofilaxis fp inner join farmacos f on fp.idFarmaco= f.id and idProfilaxis="+ idHoja.ToString(), conexion);
                using (var reader = query.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lstFarmacos.Add(new FarmacoClinica()
                        {
                            idFarmaco = Convert.ToInt32(reader["id"].ToString()),
                            nombreFarmaco = reader["nombre"].ToString(),
                            via =reader["via"].ToString(),
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
                MySqlCommand query = new MySqlCommand("select COUNT(*) as numFarmacos from farmacoprofilaxis where idProfilaxis="+ Lahoja, conexion);
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
        public  IHttpActionResult PostNewHojaFarmaco(FarmacoClinica farmaco)
        {
            if (!ModelState.IsValid)
                return BadRequest("Modelo de datos no válido");
            using (MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                conexion.Open();
                string insert = "insert into farmacoprofilaxis (idProfilaxis, idFarmaco, dosis, frecuencia, fechaAplicacion) values ("+ farmaco.idClinica+ ", "+ farmaco.idFarmaco+ ", '"+ farmaco.dosis+ "', '"+ farmaco.frecuencia+ "', '"+ farmaco.fechaAplicacion+ "' );"  ;
                MySqlCommand query = new MySqlCommand(insert, conexion);
                MySqlDataReader reader;
                reader = query.ExecuteReader();
                conexion.Close();
                return Ok();
            }
        }

        [HttpDelete]
        public IHttpActionResult Delete(int idHoja, int idFarmaco)
        {
          //  if (idFarmaco <= 0 || idHoja <= 0)
            //    return BadRequest("ID no válido");
            using (MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                conexion.Open();
                string delete = "delete from farmacoprofilaxis where idProfilaxis=" + idHoja + " and idFarmaco=" +idFarmaco;
                MySqlCommand query = new MySqlCommand(delete, conexion);
                MySqlDataReader reader;
                reader = query.ExecuteReader();
                conexion.Close();
                return Ok();
            }
        }



        [HttpPut]
        public IHttpActionResult PutFarmaco(FarmacoClinica farmaco)
        {
            if (!ModelState.IsValid)
                return BadRequest("Modelo de datos no válido");
            using (MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                conexion.Open();
                string update = "update farmacoprofilaxis set dosis='"+farmaco.dosis + "', frecuencia='"+ farmaco.frecuencia+ "' ,  fechaAplicacion='"+ farmaco.fechaAplicacion + "' where idProfilaxis='"+ farmaco.idClinica+ "'and idFarmaco='"+ farmaco.idFarmaco+ "' ;"  ;
                MySqlCommand query = new MySqlCommand(update, conexion);
                MySqlDataReader reader;
                reader = query.ExecuteReader();
                conexion.Close();
                return Ok();
            }
        }
      //  [AcceptVerbs("Delete")]
       

        public IHttpActionResult DeleteAllFarmacos(int idHoja)
        {
            if (idHoja <= 0)
                return BadRequest("Id no Válido");
            using (MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                conexion.Open();
                string delete = "delete from farmacoprofilaxis where idProfilaxis=" + idHoja;
                MySqlCommand query = new MySqlCommand(delete, conexion);
                MySqlDataReader reader;
                reader = query.ExecuteReader();
                conexion.Close();
                return Ok();
            }
        }

    }
}
