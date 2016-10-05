using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLManager
{
    public interface IQuerieable
    {
        void CreateDatabase(string dbQuery);

        DataTable ExecuteReader(QueryBuilder query, Dictionary<string, string> values = null, string log = null, bool sesion = false);

        int ExecuteNonQuery(string query, Dictionary<string, string> values = null, string log = null, bool sesion = false);
    }
}
