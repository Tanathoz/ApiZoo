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
    public class DietaController : ApiController
    {
        public IHttpActionResult GetAllDietas()
        {
            List<DietaModelView> lstDietas = new List<DietaModelView>();
            using (MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                conexion.Open();
                MySqlCommand query = new MySqlCommand("select d.idAnimal, a.nombreComun, d.cantidad, d.alimento, d.horario, d.consideraciones from dietas d inner join animal a on d.idAnimal=a.id",conexion);
                using (var reader = query.ExecuteReader()){
                    while (reader.Read())
                    {
                        lstDietas.Add(new DietaModelView()
                        {
                            idAnimal = Convert.ToInt32(reader["idAnimal"].ToString()),
                            nombreComun = reader["nombreComun"].ToString(),
                            cantidad = reader["cantidad"].ToString(),
                            alimento = reader["alimento"].ToString(),
                            horario = reader["horario"].ToString(),
                            consideraciones = reader["consideraciones"].ToString()
                        });
                    }

                }

            }

            if (lstDietas.Count == 0)
            {
                return NotFound();
            }else
            {
                return Ok(lstDietas);
            }
        }

        public IHttpActionResult GetDietaById(int id)
        {
            DietaModelView dieta = null;
            using (MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                conexion.Open();
                MySqlCommand query = new MySqlCommand("select d.idAnimal, a.nombreComun, d.cantidad, d.alimento, d.horario, d.consideraciones from dietas d inner join animal a on d.idAnimal=a.id where d.idAnimal="+id,conexion);
                using (var reader = query.ExecuteReader())
                {
                    reader.Read();
                    dieta = new DietaModelView()
                    {
                        idAnimal = Convert.ToInt32(reader["idAnimal"].ToString()),
                        nombreComun = reader["nombreComun"].ToString(),
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

        public IHttpActionResult GetDietaByConcidence(string valor)
        {
            List<DietaModelView> lstDieta = new List<DietaModelView>();
            using (MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                MySqlCommand query = new MySqlCommand("buscarDieta", conexion);
                query.CommandType = CommandType.StoredProcedure;
                query.Parameters.AddWithValue("@valor", valor);
                query.Parameters["@valor"].Direction = ParameterDirection.Input;
                conexion.Open();
                using (var reader = query.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lstDieta.Add(new DietaModelView()
                        {
                            idAnimal = Convert.ToInt32(reader["idAnimal"].ToString()),
                            nombreComun = reader["nombreComun"].ToString(),
                            cantidad = reader["cantidad"].ToString(),
                            alimento = reader["alimento"].ToString(),
                            horario = reader["horario"].ToString()
                        });
                    }
                }
                conexion.Close();
                if (lstDieta.Count == 0)
                {
                    return NotFound();
                }else
                {
                    return Ok(lstDieta);
                }
            }
        }

        public IHttpActionResult PostNewDieta(DietaModelView dieta)
        {
            if (!ModelState.IsValid)
                return BadRequest("Datos invalidos");
            using ( MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                conexion.Open();
                string insert = "insert into dietas (idAnimal, cantidad, alimento, horario, consideraciones)  values ('" + dieta.idAnimal + "', '" + dieta.cantidad + "', '" + dieta.alimento + "', '" + dieta.horario + "','" + dieta.consideraciones + "' );";
                MySqlCommand query = new MySqlCommand(insert, conexion);
                MySqlDataReader myReader;
                myReader = query.ExecuteReader();
                conexion.Close();
                return Ok();
            }
        }

        public IHttpActionResult PutDieta(DietaModelView dieta)
        {
            if (!ModelState.IsValid)
                return BadRequest(" Datos invalidos");
            using ( MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                conexion.Open();
                string update = "update dietas set cantidad='"+ dieta.cantidad+ "' , alimento='"+dieta.alimento+ "', horario='"+ dieta.horario+ "',consideraciones='"+dieta.consideraciones+ "' where idAnimal='"+dieta.idAnimal+ "';" ;
                MySqlCommand query = new MySqlCommand(update, conexion);
                MySqlDataReader myReader;
                myReader = query.ExecuteReader();
                conexion.Close();
                return Ok();

            }
        }

        public IHttpActionResult Delete(int  idAnimal)
        {
            if (idAnimal <= 0)
                return BadRequest("Id no válido error ");
            using (MySqlConnection conexion = ConexionBase.GetDBConnection())
            {
                conexion.Open();
                string delete = "delete from dietas  where idAnimal=" + idAnimal.ToString();
                MySqlCommand query = new MySqlCommand(delete, conexion);
                MySqlDataReader myReader;
                myReader = query.ExecuteReader();
                conexion.Close();
                return Ok();
            }
        }





    }

   
}


