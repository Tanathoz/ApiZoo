using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
namespace Conexion
{
    public class ConexionBase
    {

        /* static  string host = "chilpan.mysql.database.azure.com";
         static int port = 3306;
         static string database = "Zoochilpan"; utf8_general_ci	
         static string username = "Tanathoz@chilpan";
         static string password = "Itachi313";*/

        static string host = "mysql5009.site4now.net";
        static int port = 3306;
        static string database = "db_a463da_zoo";
        static string username = "a463da_zoo";
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