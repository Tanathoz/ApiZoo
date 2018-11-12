using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
namespace Conexion
{
    public class ConexionBase
    {
        static  string host = "127.0.0.1";
        static int port = 3306;
        static string database = "zoochiloan";
        static string username = "root";
        static string password = "";

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