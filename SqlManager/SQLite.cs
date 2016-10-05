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

        public static int ExecuteNonQuery(string query)
        {
            ValidateConnectionString();

            int n;
            Debug.WriteLine(query);

            using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                var transaction = conn.BeginTransaction();
                SQLiteCommand cmd = new SQLiteCommand(query, conn);

                cmd.Parameters.AddRange(Parameters.ToArray());

                n = cmd.ExecuteNonQuery();
                transaction.Commit();
            }

            return n;
        }

        public static DataTable ExecuteReader(QueryBuilder query)
        {
            ValidateConnectionString();

            DataTable dt = new DataTable();
            string _query = query.ToString();
            Debug.WriteLine(_query);

            using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(_query, conn);

                cmd.Parameters.AddRange(Parameters.ToArray());

                dt.Load(cmd.ExecuteReader());
            }

            return dt;
        }
    }
}
