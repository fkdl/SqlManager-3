using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLManager
{
    public class SQLite : SQLHandler
    {
        private static void ValidateConnectionString()
        {
            throw new NotImplementedException();
        }

        public static void CreateDatabase(string dbQuery)
        {
            ValidateConnectionString();

            using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                var transaction = conn.BeginTransaction();
                SQLiteCommand cmd = new SQLiteCommand(dbQuery, conn);

                cmd.ExecuteNonQuery();
                transaction.Commit();
            }
        }

        public static int ExecuteNonQuery(string query, Dictionary<string, string> values = null)
        {
            ValidateConnectionString();

            int n;
            Debug.WriteLine(query);

            using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                var transaction = conn.BeginTransaction();
                SQLiteCommand cmd = new SQLiteCommand(query, conn);

                if (values != null)
                {
                    foreach (var key in values.Keys)
                    {
                        cmd.Parameters.Add(new SQLiteParameter(key, values[key]));
                    }
                }

                foreach (SQLiteParameter param in cmd.Parameters)
                {
                    if (param.Value == null)
                        param.Value = DBNull.Value;
                }

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

            using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(_query, conn);

                if (values != null)
                {
                    foreach (var key in values.Keys)
                    {
                        cmd.Parameters.Add(new SQLiteParameter(key, values[key]));
                    }
                }
                
                dt.Load(cmd.ExecuteReader());
            }

            return dt;
        }
    }
}
