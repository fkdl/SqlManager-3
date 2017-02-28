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
        public static bool ShowQueryOnConsole { get; set; }

        public static void CreateConnectionString(string dbFilePath, int sqliteVersion = 3)
        {
            string connectionString = $"Data Source = {dbFilePath}; Version = {sqliteVersion}";
            ConnectionString = connectionString.ToString();
        }

        private static void ValidateConnectionString()
        {
            if (string.IsNullOrEmpty(ConnectionString) || string.IsNullOrWhiteSpace(ConnectionString))
            {
                throw new Exception("Use command CreateConnectionString() before attempting to query it.");
            }
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

        public static DataTable GetDataTable(string table, params string[] columns)
        {
            var dt = new DataTable();
            string columnsFormatted = "*";

            if(columns.Length > 0)
            {
                columnsFormatted = string.Join(",", columns);
            }

            var query = new QueryBuilder()
                .Select(columnsFormatted)
                .From(table);
            
            dt = ExecuteReader(query);

            return dt;
        }

        public static bool Insert(string table, Dictionary<string, string> data)
        {
            bool returnCode = true;
            var parameters = GenerateSQLiteParameters(data);
            string columns = GetColumns(data);
            string paramsKeys = GetParametersKeys(data);
            
            try
            {
                ExecuteNonQuery($"INSERT INTO {table}({columns}) VALUES ({paramsKeys});", parameters);
            }
            catch (Exception)
            {
                returnCode = false;
            }

            return returnCode;
        }

        public static bool Update(string table, Dictionary<string, string> data, string where)
        {
            bool returnCode = true;
            var parameters = GenerateSQLiteParameters(data);
            string updateColumns = string.Empty;

            foreach (KeyValuePair<string, string> val in data)
            {
                updateColumns += $"{val.Key} = @{val.Key},";
            }

            updateColumns = updateColumns.Substring(0, updateColumns.Length - 1);

            try
            {
                ExecuteNonQuery($"UPDATE {table} SET {updateColumns} WHERE {where}", parameters);
            }
            catch (Exception)
            {
                returnCode = false;
            }

            return returnCode;
        }

        private static int ExecuteNonQuery(string query, List<SQLiteParameter> values)
        {
            ValidateConnectionString();

            int n;
            DisplayQueryOnConsole(query);

            using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                var transaction = conn.BeginTransaction();
                SQLiteCommand cmd = new SQLiteCommand(query, conn);

                cmd.Parameters.AddRange(values.ToArray());

                n = cmd.ExecuteNonQuery();
                transaction.Commit();
            }

            return n;
        }

        private static DataTable ExecuteReader(QueryBuilder query)
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

        private static List<SQLiteParameter> GenerateSQLiteParameters(Dictionary<string, string> data)
        {
            List<SQLiteParameter> parameters = new List<SQLiteParameter>();

            foreach (KeyValuePair<string, string> val in data)
            {
                parameters.Add(new SQLiteParameter()
                {
                    ParameterName = $"@{val.Key}",
                    Value = val.Value
                });
            }

            return parameters;
        }

        private static string GetColumns(Dictionary<string, string> _data)
        {
            string _columns = string.Empty;

            foreach (KeyValuePair<string, string> val in _data)
            {
                _columns += $"{val.Key},";
            }

            _columns = _columns.Substring(0, _columns.Length - 1);

            return _columns;
        }

        private static string GetParametersKeys(Dictionary<string, string> _data)
        {
            string _paramKeys = string.Empty;

            foreach (KeyValuePair<string, string> val in _data)
            {
                _paramKeys += $"@{val.Key},";
            }

            _paramKeys = _paramKeys.Substring(0, _paramKeys.Length - 1);

            return _paramKeys;
        }

        private static void DisplayQueryOnConsole(string _query)
        {
            if (ShowQueryOnConsole)
            {
                Console.WriteLine(_query);
            }
        }
    }
}
