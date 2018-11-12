using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Conexion.Models;
using MySql.Data.MySqlClient;
namespace Conexion.Controllers
{
    public class VeterinarioController : ApiController
    {
        public IHttpActionResult GetAllVeterinarios()
        {
            IList<VeterinarioModel> lstVeterinarios = null;
            //opcion origilan List<VeterinarioModel> lstVeterinarios = new  List<VeterinarioModel>();
            using ( MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                conexion.Open();
                MySqlCommand query = new MySqlCommand("select id, nombre, apellidoPaterno, apellidoMaterno, sexo from veterinarios", conexion);
                using (var reader = query.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lstVeterinarios.Add(new VeterinarioModel()
                        {
                            id = Convert.ToInt32(reader["Id"]),
                            nombre = reader["nombre"].ToString(),
                            apellidoPaterno = reader["apellidoPaterno"].ToString(),
                            apellidoMaterno = reader["apellidoMaterno"].ToString(),
                            sexo = reader["sexo"].ToString()
                        });
                    }
                }
            }

            if (lstVeterinarios.Count == 0)
            {
                return NotFound();

            } else
            {
                return Ok(lstVeterinarios);
            }
        }
    }
}
