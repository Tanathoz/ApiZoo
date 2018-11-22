using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
namespace Conexion
{
    public class ConexionBase
    {
        static  string host = "mysql5019.site4now.net";
        static int port = 3306;
        static string database = "db_a42b84_zoo";
        static string username = "a42b84_zoo";
        static string password = "itachi313";

        public static MySqlConnection GetDBConnection()
        {
            String connString = "Server="+host+";Database=" + database
                + ";port=" + port + ";User Id=" + username + ";password=" + password;

            MySqlConnection conex = new MySqlConnection(connString);
            Console.WriteLine("HAAHAH MAGAZO L");// comment

            return conex;
        }
    }
}