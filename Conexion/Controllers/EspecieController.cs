using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MySql.Data.MySqlClient;
using System.Data;
using Conexion.Models;
namespace Conexion.Controllers
{
    
    public class EspecieController : ApiController
    {
        
        public IHttpActionResult GetAllEspecies()
        {
            List<EspecieViewModel> lstEspecies = new List<EspecieViewModel>();
            using (MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                conexion.Open();
                MySqlCommand query = new MySqlCommand("select * from especies", conexion);
                using (var reader = query.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lstEspecies.Add(new EspecieViewModel()
                        {
                            idEspecie = Convert.ToInt32(reader["idEspecie"]),
                            idFamilia = Convert.ToInt32(reader["idFamilia"]),
                            nombre = reader["nombre"].ToString()

                        });
                    }
                }
            }

            if ( lstEspecies.Count == 0)
            {
                return NotFound();
            }else
            {
                return Ok(lstEspecies);
            }

        }

        public IHttpActionResult PostNewEspecie(EspecieViewModel especie)
        {
            if (!ModelState.IsValid)
                return BadRequest("Datos invalidos");

            using (MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                conexion.Open();
                string insert = "insert into especies(idEspecie, idFamilia, nombre) values (" + especie.idEspecie + ", " + especie.idFamilia + ", '" + especie.nombre + "' );";
                MySqlCommand query = new MySqlCommand(insert, conexion);
                MySqlDataReader myReader;
                myReader = query.ExecuteReader();
                conexion.Close();
                return Ok();
            }
        }

        //obtener especies por ID falta
    }
}
