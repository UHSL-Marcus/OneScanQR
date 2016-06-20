using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace SQLControls
{
    public class DatabaseTableObject
    {
        public int? Id;
    }

    public enum JoinTypes
    {
        JOIN
    }

    

    public class JoinOperators
    {
        public readonly static string EQUALS = "=";
    }
    public class DatabaseOutputObjectJoin
    {
        public JoinTypes join;
        public string left;
        public string right;
        public string leftColumn;
        public string rightColumn;
        public string op;

        public DatabaseOutputObjectJoin(JoinTypes join, string leftTable, string leftColumn, string rightTable, string rightColumn, string op)
        {
            this.join = join;
            left = leftTable;
            this.leftColumn = leftColumn;
            right = rightTable;
            this.rightColumn = rightColumn;
        }

    }
    
    public abstract class DatabaseOutputObject
    {
        protected List<DatabaseTableObject> whereObjects = new List<DatabaseTableObject>();
        protected List<DatabaseOutputObjectJoin> joinObjects = new List<DatabaseOutputObjectJoin>();
        protected string FROM;
        protected static DatabaseTableObject createWhereObject(string table, string column, string info)
        {
           return SharedUtils.buildDatabaseObjectSingleField(table, info, column);
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method)]
    public class DatabaseOutputAttribute : Attribute
    {
        public bool SQLIgnore = false;
        public string columnName;
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

        internal static List getData(SqlCommand cmd, string column)
        {
            DataTableReader reader = SharedUtils.getDataReader(cmd);

            List returnList = new List();

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
        
        internal static List getData(string sql)
        {

            return getData(new SqlCommand(sql));
        }

        internal static List getData(SqlCommand cmd)
        {
            DataTableReader reader = SharedUtils.getDataReader(cmd);

            List returnList = new List();

            while (reader.Read())
            {

                DatabaseTableObject ob = Activator.CreateInstance();

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

    }
}
