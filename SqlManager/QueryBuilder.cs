using System.Collections.Generic;
using System.Text;

namespace SQLManager
{
    public enum JoinType
    {
        Left,
        Right,
        Inner
    }

    public class QueryBuilder
    {
        List<string> columns = new List<string>();
        List<string> tables = new List<string>();
        List<string> joins = new List<string>();
        List<string> where = new List<string>();
        List<string> orderBy = new List<string>();
        List<QueryBuilder> union = new List<QueryBuilder>();
        int limit = -1;

        public override string ToString()
        {
            StringBuilder query = new StringBuilder();

            if(columns.Count > 0 && tables.Count > 0)
            {
                query.Append("SELECT ");

                //COLUMNS
                for (int i = 0; i < columns.Count; i++)
                {
                    if (i < columns.Count - 1)
                        query.Append($"{columns[i]},");
                    else
                        query.Append($"{columns[i]} ");
                }

                //TABLES
                query.Append("FROM ");
                for (int i = 0; i < tables.Count; i++)
                {
                    if (i < tables.Count - 1)
                        query.Append($"{tables[i]},");
                    else
                        query.Append($"{tables[i]} ");
                }

                //JOIN
                for (int i = 0; i < joins.Count; i++)
                {
                    query.Append($"{joins[i]} ");
                }

                //WHERE
                for (int i = 0; i < where.Count; i++)
                {
                    if (i == 0)
                        query.Append($"WHERE {where[i]} ");
                    else
                        query.Append($"AND {where[i]} ");
                }

                //LIMIT
                if(limit > 0)
                {
                    query.Append($"LIMIT {limit}");
                }

                //ORDER BY
                if(orderBy.Count > 0)
                {
                    query.Append("ORDER BY ");
                    for (int i = 0; i < orderBy.Count; i++)
                    {
                        if (i < orderBy.Count - 1)
                            query.Append($"{orderBy[i]},");
                        else
                            query.Append($"{orderBy[i]} ");
                    }
                }

                //UNION
                if(union.Count > 0)
                {
                    for (int i = 0; i < union.Count; i++)
                    {
                        query.Append("UNION ");
                        query.Append(union[i].ToString());
                    }
                }
            }

            return query.ToString();
        }

        public QueryBuilder Select(params string[] _columns)
        {
            columns.AddRange(_columns);
            return this;
        }

        public QueryBuilder From(params string[] _tables)
        {
            tables.AddRange(_tables);
            return this;
        }

        public QueryBuilder Where(params string[] _where)
        {
            where.AddRange(_where);
            return this;
        }

        public QueryBuilder OrderBy(string column, string type = "ASC")
        {
            if (type != "ASC" && type != "DESC")
            {
                type = "ASC";
            }

            orderBy.Add($"{column} {type.ToString()}");
            return this;
        }

        public QueryBuilder Join(string table, JoinType type = JoinType.Inner, params string[] on)
        {
            StringBuilder join = new StringBuilder();

            if (type == JoinType.Left)
            {
                join.Append("LEFT JOIN ");
            } else if (type == JoinType.Right)
            {
                join.Append("RIGHT JOIN ");
            } else
            {
                join.Append("INNER JOIN ");
            }

            join.Append($"{table} ");

            for (int i = 0; i < on.Length; i++)
            {
                if (i == 0)
                    join.Append($"ON {on[i]} ");
                else
                    join.Append($"AND {on[i]} ");
            }

            joins.Add(join.ToString());

            return this;
        }

        public QueryBuilder Limit(int _limit)
        {
            limit = _limit;
            return this;
        }

        public QueryBuilder Union(QueryBuilder query)
        {
            union.Add(query);
            return this;
        }

        public QueryBuilder Clear()
        {
            columns.Clear();
            tables.Clear();
            where.Clear();
            orderBy.Clear();

            return this;
        }
    }
}
