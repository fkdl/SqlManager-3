using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
