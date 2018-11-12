using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySql.Data.MySqlClient;
namespace Conexion.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            Console.WriteLine("Getting Connection");
            MySqlConnection conn = ConexionBase.GetDBConnection();
            try
            {
                Console.WriteLine("Openning Conecction..");
                conn.Open();
                Console.WriteLine("Success connection");
            }catch (Exception e)
            {
                Console.WriteLine("Error porpa"+e.Message);
            }

            return View();
        }
    }
}
