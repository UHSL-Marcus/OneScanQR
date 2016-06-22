using StringEnum;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace SQLControls
{
    
    public enum SQLWhereConjuctions
    {
        [StringValue("AND")]
        AND,
        [StringValue("OR")]
        OR,
        [StringValue("NOT")]
        NOT
    }
    public enum SQLEqualityOperations
    {
        [StringValue("=")]
        EQUALS,
        [StringValue("<>")]
        NOTEQUALS,
        [StringValue("IS NULL")]
        ISNULL,
        [StringValue("IS NOT NULL")]
        NOTNULL
    }
    public class DatabaseTableObject
    {
        public int? Id;

        [DatabaseColumn(SQLIgnore = true)]
        public SQLEqualityOperations Equality = SQLEqualityOperations.EQUALS;
        [DatabaseColumn(SQLIgnore = true)]
        public bool TreatAsNull = false;

        
            
    }

    public struct JoinPair
    {
        public Type leftTable;
        public Type rightTable;
        public JoinOnPair[] ons;
        public JoinPair(Type leftTable, Type rightTable, JoinOnPair[] ons)
        {
            this.leftTable = leftTable;
            this.rightTable = rightTable;
            this.ons = ons;
        }
    }

    public struct JoinOnPair
    {
        public string leftTableCol;
        public string rightTableCol;
        public SQLEqualityOperations op;
        public SQLWhereConjuctions conjunc;
        public JoinOnPair(string leftTableColumn, string rightTableColumn, SQLEqualityOperations op = SQLEqualityOperations.EQUALS, SQLWhereConjuctions conjuction = SQLWhereConjuctions.AND)
        {
            leftTableCol = leftTableColumn;
            rightTableCol = rightTableColumn;
            this.op = op;
            conjunc = conjuction;
        }
    }

    public abstract class DatabaseOutputObject
    {
        public List<JoinPair> joins = new List<JoinPair>();
        public List<Tuple<DatabaseTableObject, SQLWhereConjuctions, bool>> whereobs = new List<Tuple<DatabaseTableObject, SQLWhereConjuctions, bool>>();

        protected void buildSingleJoin(string left, string right, string leftcol, string rightcol)
        {
            JoinOnPair[] ons = new JoinOnPair[] { new JoinOnPair(leftcol, rightcol) };
            joins.Add(new JoinPair(SharedUtils.buildDatabaseObjectNoFields(left).getType(), SharedUtils.buildDatabaseObjectNoFields(right).getType(), ons));
        }
        protected void buildSingleWhere(string table, string column, object info)
        {
            whereobs.Add(Tuple.Create(SharedUtils.buildDatabaseObjectSingleField(table, info, column), SQLWhereConjuctions.AND, false));
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class DatabaseColumnAttribute : Attribute
    {
        public bool SQLIgnore = false;
        public string columnName;
        public SQLEqualityOperations equality = SQLEqualityOperations.EQUALS;
        public bool forceUse = false;
    }
    

    internal class SharedUtils
    {
        public static bool doNonQuery(string sql)
        {
            SqlCommand cmd = new SqlCommand(sql);
            return doNonQuery(cmd);

        }

        public static bool doNonQuery(SqlCommand cmd)
        {
            bool success = false;

            using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.Database))
            {
                conn.Open();
                cmd.Connection = conn;

                if (cmd.ExecuteNonQuery() != 0)
                    success = true;
            }

            return success;
        }

        internal static bool getSingleEntry<T>(SqlCommand cmd, string columnName, out T output)
        {
            bool success = false;
            output = default(T);

            List<T> entries = getData<T>(cmd, columnName);
            if (entries.Count > 0)
            {
                output = entries[0];
                success = true;
            }

            return success;
        }

        internal static bool getSingleEntry<T>(string sql, string columnName, out T output)
        {
            return getSingleEntry(new SqlCommand(sql), columnName, out output);
        }

        internal static List<TYPE> getData<TYPE>(SqlCommand cmd, string column)
        {
            DataTableReader reader = SharedUtils.getDataReader(cmd);

            List<TYPE> returnList = new List<TYPE>();

            while (reader.Read())
            {

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    if (reader.GetName(i).Equals(column))
                    {
                        var entry = reader[i];
                        if (entry is TYPE)
                            returnList.Add((TYPE)reader[i]);
                    }
                }
            }

            return returnList;
        }


        internal static List<Dictionary<string, object>> getData(SqlCommand cmd)
        {
            DataTableReader reader = getDataReader(cmd);

            List<Dictionary<string, object>> returnList = new List<Dictionary<string, object>>();
            while (reader.Read())
            {
                Dictionary<string, object> row = new Dictionary<string, object>();

                for (int i = 0; i < reader.FieldCount; i++)
                    row.Add(reader.GetName(i), reader[i]);
            }

            return returnList;
        }

        internal static List<TYPE> getData<TYPE>(string sql)
        {

            return getData<TYPE>(new SqlCommand(sql));
        }


        internal static List<TYPE> getData<TYPE>(SqlCommand cmd)
        {
            DataTableReader reader = getDataReader(cmd);

            List<TYPE> returnList = new List<TYPE>();

            while (reader.Read())
            {

                TYPE ob = (TYPE)Activator.CreateInstance(typeof(TYPE));

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string s = reader.GetName(i);
                    FieldInfo field = ob.GetType().GetField(reader.GetName(i));
                    if (field.FieldType == typeof(string[]))
                        field.SetValue(ob, ((string)reader[i]).Split(','));
                    else
                        field.SetValue(ob, reader[i]);
                }

                returnList.Add(ob);
            }

            return returnList;
        }


        internal static DataTableReader getDataReader(SqlCommand cmd)
        {
            DataSet dataSet = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter();

            using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.Database))
            {
                cmd.Connection = conn;
                adapter.SelectCommand = cmd;
                conn.Open();
                adapter.Fill(dataSet);

            }

            return dataSet.CreateDataReader();
        }

        internal static DataTableReader getDataReader(string sql)
        {
            return getDataReader(new SqlCommand(sql));
        }



        internal static object formatValue(object value)
        {
            if (value is DateTime)
                return ((DateTime)value).ToString("yyyy/MM/dd HH:mm:ss");
            else if (value is int || value is string)
                return value;
            else if (value is string[])
                return string.Join(",", value as string[]);

            return null;
        }

        internal static dynamic buildDatabaseObject(string table, Dictionary<string, object> values)
        {
            List<DatabaseTypeBuilder.Field> newFields = new List<DatabaseTypeBuilder.Field>();
            for (int i = 0; i < values.Keys.Count; i++)
            {
                string key = values.Keys.ElementAt(i);
                Type type = values[key].GetType();
                if (type == typeof(int)) type = typeof(int?); // to allow nulls
                newFields.Add(new DatabaseTypeBuilder.Field(key, values[key].GetType()));
            }

            Type tempType = DatabaseTypeBuilder.GetType(table, newFields.ToArray());

            dynamic ob = Convert.ChangeType(Activator.CreateInstance(tempType), tempType);

            foreach (FieldInfo field in tempType.GetFields())
            {
                object value;
                values.TryGetValue(field.Name, out value);
                field.SetValue(ob, value);
            }

            return ob;
        }

        internal static dynamic buildDatabaseObjectSingleField(string table, object info, string column)
        {
            Dictionary<string, object> conditions = new Dictionary<string, object>();
            conditions.Add(column, info);
            return SharedUtils.buildDatabaseObject(table, conditions);
        }

        internal static dynamic buildDatabaseObjectNoFields(string table)
        {
            Dictionary<string, object> conditions = new Dictionary<string, object>();
            return SharedUtils.buildDatabaseObject(table, conditions);
        }

        internal static string getWhere(Tuple<DatabaseTableObject, SQLWhereConjuctions, bool>[] obs, ref SqlCommand cmd, string paramPrefix)
        {
            string query = "";

            for (int t = 0; t < obs.Length; t++)
            {
                Tuple<DatabaseTableObject, SQLWhereConjuctions, bool> ob = obs[t];
                
                Type type = ob.GetType();

                FieldInfo[] fields = type.GetFields();
                if (fields.Length > 0)
                {
                    if (t > 0 || ob.Item2 == SQLWhereConjuctions.NOT)
                        query += " " + ob.Item2.GetStringValue() + " ";

                    query += "(";
                    for (int i = 0; i < fields.Length; i++)
                    {
                        if (!sqlIgnore(fields[i]))
                        {
                            var value = formatValue(fields[i].GetValue(ob.Item1));

                            if (value != null || ob.Item3)
                            {
                                SqlParameter tempParam = new SqlParameter();
                                tempParam.ParameterName = "@" + paramPrefix + Regex.Replace(fields[i].Name, "[^A-Za-z0-9 _]", "");

                                SQLEqualityOperations equality = ob.Item1.Equality;

                                if (value is string)
                                    tempParam.Value = ((string)value).Trim();
                                if (value == null)
                                {
                                    tempParam.Value = "";
                                    switch(equality)
                                    {
                                        case SQLEqualityOperations.EQUALS:
                                            equality = SQLEqualityOperations.ISNULL;
                                            break;
                                        case SQLEqualityOperations.NOTEQUALS:
                                            equality = SQLEqualityOperations.NOTNULL;
                                            break;
                                    }
                                }
                                else tempParam.Value = value;

                                cmd.Parameters.Add(tempParam);

                                query += type.Name + "." + fields[i].Name + equality.GetStringValue() + tempParam.ParameterName;

                                if (i + 1 < fields.Length)
                                    query += " AND ";
                            }
                        }
                    }
                    query += ")";
                }
            }

            return query;
        }

        private static bool sqlIgnore(FieldInfo field)
        {
            bool ignore = false;
            DatabaseColumnAttribute[] attrs = field.GetCustomAttributes(typeof(DatabaseColumnAttribute), false) as DatabaseColumnAttribute[];
            if (attrs.Length > 0)
                ignore = attrs[0].SQLIgnore;

            return ignore;
        }

    }
}
