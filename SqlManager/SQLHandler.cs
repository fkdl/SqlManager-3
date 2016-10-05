using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SQLManager
{
    public abstract class SQLHandler
    {
        private static string connectionString = String.Empty;

        public static string ConnectionString
        {
            protected get { return connectionString; }
            set
            {
                if (connectionString == String.Empty)
                    connectionString = value;
            }
        }

        public static List<SqlParameter> Parameters = new List<SqlParameter>();

        public static void AddParameter(string key, object value)
        {
            Parameters.Add(new SqlParameter(key, value));
        }

        public static void AddParameter(KeyValuePair<string, object> pms)
        {
            Parameters.Add(new SqlParameter(pms.Key, pms.Value));
        }

        public static void AddRangeParameters(List<KeyValuePair<string, object>> pms)
        {
            foreach (var p in pms)
            {
                Parameters.Add(new SqlParameter(p.Key, p.Value));
            }
        }
    }
}
