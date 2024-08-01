using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Sql;
using System.Data.SqlClient;
namespace WebApplication1.Models
{
    public class DbAccess
    {
        static string ConntectionString= @"Data Source=DESKTOP-44PAKFG\MSSQLSERVER02; initial catalog=WebApp;integrated Security=true";
        public  SqlConnection con = new SqlConnection(ConntectionString);
        public  SqlCommand cmd = null;

        public void OpenCon()
        {
              if (con.State == System.Data.ConnectionState.Closed)
            {
                con.Open();
            }
        }

        public void CloseCon()
        {
              if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
        }
        public void InsertUpdateDelete(string q)
        {
            OpenCon();
            cmd = new SqlCommand(q, con);
            cmd.ExecuteNonQuery();
            CloseCon();
        }
        
    }
}