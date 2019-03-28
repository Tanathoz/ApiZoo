using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using MySql.Data.MySqlClient;
using Conexion.Models;
using System.Web.Http;
using System.Data;
using System.Web.Http.Cors;

namespace Conexion.Controllers
{
    public class FarmacoController : ApiController
    {
      //  [EnableCors(origins: "http://localhost:64155/", headers: "*", methods: "*")]
        public IHttpActionResult GetAllFarmacos()
        {
            List<FarmacoViewModel> lstFarmacos = new List<FarmacoViewModel>();
            using (MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                conexion.Open();
                MySqlCommand query = new MySqlCommand(" select id, nombre, via from farmacos", conexion);
                using (var reader = query.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lstFarmacos.Add(new FarmacoViewModel
                        {
                            id= Convert.ToInt32(reader["id"].ToString()),
                            nombre = reader["nombre"].ToString(),
                            via = reader["via"].ToString()
                        });
                    }
                }

                if (lstFarmacos.Count == 0)
                {
                    return NotFound();
                }else
                {
                    return Ok(lstFarmacos);
                }
            }

           
        }

       // [EnableCors(origins: "http://localhost:64155/", headers: "*", methods: "*")]
        public IHttpActionResult GetFarmacoById(int id)
        {
            FarmacoViewModel farmaco = null;
            using (MySqlConnection conexion= ConexionBase.GetDBConnection())
            {
                conexion.Open();
                MySqlCommand query = new MySqlCommand(" select id, nombre, via from farmacos where id="+id.ToString(),conexion);
                using (var reader = query.ExecuteReader())
                {
                    reader.Read();
                    farmaco = new FarmacoViewModel() {
                        id = Convert.ToInt32(reader["id"].ToString()),
                        nombre = reader["nombre"].ToString(),
                        via = reader["via"].ToString()
                    };

                }

                if (farmaco == null)
                    return NotFound();
                else
                    return Ok(farmaco);
            }
        }


        public IHttpActionResult GetFarmacoByConcidence (string valor)
        {
            List<FarmacoViewModel> lstFarmacos = new List<FarmacoViewModel>();
            using (MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                MySqlCommand query = new MySqlCommand("buscarFarmaco", conexion);
                query.CommandType = CommandType.StoredProcedure;
                query.Parameters.AddWithValue("@valor", valor);
                query.Parameters["@valor"].Direction = ParameterDirection.Input;
                conexion.Open();
                using (var reader = query.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lstFarmacos.Add(new FarmacoViewModel
                        {
                            id = Convert.ToInt32(reader["id"].ToString()),
                            nombre = reader["nombre"].ToString(),
                            via = reader["via"].ToString()
                        });
                    }
                }
                conexion.Close();
                if (lstFarmacos.Count == 0)
                {
                    return NotFound();
                }else
                {
                    return Ok(lstFarmacos);
                }
            }
        }

     //   [EnableCors(origins: "http://localhost:64155/", headers:"*", methods:"*")]
        public IHttpActionResult PostNewFarmaco(FarmacoViewModel farmaco)
        {
            if (!ModelState.IsValid)
                return BadRequest("Datos invalidos");
            using (MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                conexion.Open();
                string insert = "insert into farmacos (id, nombre,via ) values ('" + farmaco.id + "',  '" + farmaco.nombre + "', '" + farmaco.via + "' );";
                MySqlCommand query = new MySqlCommand(insert, conexion);
                MySqlDataReader myReader;
                myReader = query.ExecuteReader();
                conexion.Close();
                return Ok();

            }
        }
     //   [EnableCors(origins: "http://localhost:64155/", headers: "*", methods: "*")]
        public IHttpActionResult PutFarmaco(FarmacoViewModel farmaco)
        {
            if (!ModelState.IsValid)
                return BadRequest("Modelo de datos invalido");
            using (MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                conexion.Open();
                string update = "update farmacos set nombre='" + farmaco.nombre + "', via='" + farmaco.via + "' where id='" + farmaco.id + "'; ";
                MySqlCommand query = new MySqlCommand(update, conexion);
                MySqlDataReader reader;
                reader = query.ExecuteReader();
                return Ok();

            }
        }

     //   [EnableCors(origins: "http://localhost:64155/", headers: "*", methods: "*")]
        public IHttpActionResult Delete(int id)
        {
            if (id <= 0)
                return BadRequest("ID de farmaco invalido");
            using (MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                conexion.Open();
                string delete = "delete from farmacos where id="+id.ToString();
                MySqlCommand QUERY = new MySqlCommand(delete, conexion);
                MySqlDataReader reader;
                reader = QUERY.ExecuteReader();
                conexion.Close();
                return Ok();
            }
        }





    }
}
