using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using System.Diagnostics;

namespace SQLManager
{
    public class MySQL : SQLHandler
    {
        public static void CreateConnectionString(string server, string user, string pwd, string db)
        {
            MySqlConnectionStringBuilder connectionString = new MySqlConnectionStringBuilder();

            connectionString.Server = server;
            connectionString.UserID = user;
            connectionString.Password = pwd;
            connectionString.Database = db;

            ConnectionString = connectionString.ToString();
        }

        public static void ValidateConnectionString()
        {
            if(ConnectionString == String.Empty)
            {
                throw new Exception($"Invalid Connection string: {ConnectionString}");
            }
        }

        public static void CreateDatabase(string dbQuery)
        {
            ValidateConnectionString();

            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                var transaction = conn.BeginTransaction();
                MySqlCommand cmd = new MySqlCommand(dbQuery, conn);

                cmd.ExecuteNonQuery();
                transaction.Commit();
            }
        }

        public static int ExecuteNonQuery(string query)
        {
            ValidateConnectionString();

            int n;
            Debug.WriteLine(query);

            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                var transaction = conn.BeginTransaction();
                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddRange(Parameters.ToArray());

                n = cmd.ExecuteNonQuery();
                transaction.Commit();
            }

            return n;
        }

        public static DataTable ExecuteReader(QueryBuilder query, Dictionary<string, string> values = null)
        {
            ValidateConnectionString();

            DataTable dt = new DataTable();
            string _query = query.ToString();
            Debug.WriteLine(_query);

            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(_query, conn);

                cmd.Parameters.AddRange(Parameters.ToArray());

                dt.Load(cmd.ExecuteReader());
            }

            return dt;
        } 
    }
}
