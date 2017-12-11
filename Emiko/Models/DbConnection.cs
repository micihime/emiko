using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace Emiko.Models
{
    public class DbConnection
    {
        private static string connectionString = "Data Source=tcp:dmdod4qxxj.database.windows.net,1433;Initial Catalog=haiku;User Id=mitchie@dmdod4qxxj;Password=Databaza1";
        private SqlConnection connection = new SqlConnection(connectionString);

        public SqlConnection Connection
        {
            get { return connection; }
            set { connection = value; }
        }

        public void connect() 
        {
            connection.Close();
            connection.Open(); //open the connection to the database
        }
        
        public void execute(SqlCommand cmd)
        {
            connect();
            cmd.Connection = connection; //add connection to SQL command
            cmd.ExecuteNonQuery(); //execute SQL command
            disconnect();
        }

        public SqlDataReader read(SqlCommand cmd)
        {
            connect();
            cmd.Connection = connection; //add connection to SQL command
            SqlDataReader reader = cmd.ExecuteReader(); //execute SQL reader
            //cannot disconnect from database here, because also the reader will be closed and returned reader will be empty
            return reader;
        }

        public void disconnect()
        {
            connection.Close(); //close the connection to the database
        }
    }
}