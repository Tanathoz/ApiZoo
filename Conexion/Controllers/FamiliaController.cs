using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Conexion.Models;
using System.Data;
using MySql.Data.MySqlClient;
namespace Conexion.Controllers
{
    public class FamiliaController : ApiController
    {

        public IHttpActionResult GetAllFamilias()
        {
            List<FamiliaViewModel> lstFamilias = new List<FamiliaViewModel>();
            using (MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                conexion.Open();
                MySqlCommand query = new MySqlCommand("select * from familias", conexion);
                using (var reader = query.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lstFamilias.Add(new FamiliaViewModel()
                        {
                            idFam = Convert.ToInt32(reader["idFam"]),
                            idOrden = Convert.ToInt32(reader["idOrden"]),
                            nombre = reader["nombre"].ToString()

                        });
                    }
                }
            }
            if (lstFamilias.Count == 0)
            {
                return NotFound();
            }
            else
            {
                return Ok(lstFamilias);
            }
        }

        public IHttpActionResult PostNewFamilia(FamiliaViewModel familia)
        {
            if (!ModelState.IsValid)
                return BadRequest("Datos invalidos");

            using (MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                conexion.Open();
                string insert = "insert into familias(idFam, idOrden, nombre) values ('" + familia.idFam + "', " + familia.idOrden + ", '" + familia.nombre + "' );";
                MySqlCommand query = new MySqlCommand(insert, conexion);
                MySqlDataReader myReader;
                myReader = query.ExecuteReader();
                conexion.Close();
                return Ok();
            }

        }

        public IHttpActionResult GetFamiliaByIdOrden(int id)
        {
            List<FamiliaViewModel> lstFamilias = new List<FamiliaViewModel>();
       
            using (MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                conexion.Open();
                MySqlCommand query = new MySqlCommand("select  idFam, nombre from familias where idOrden=" + id.ToString(), conexion);
                using (var reader = query.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lstFamilias.Add(new FamiliaViewModel()
                        {

                            idFam = Convert.ToInt32(reader["idFam"]),
                            idOrden= id,
                            nombre = reader["nombre"].ToString()
                        });

                    }
                    // try
                    //{
                    

                    // }
                    /* catch (Exception e)
                     {
                         Console.WriteLine("ERRORSS"+e.Message);
                     } */

                }

                if (lstFamilias.Count == 0)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(lstFamilias);
                }
            }
        }
    }

}
