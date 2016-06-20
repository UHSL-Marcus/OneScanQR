using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SQLControls
{
    public class Delete
    {
        public static bool doDeleteByIDGetID(string id, string table, out int? output)
        {
            return doDeleteByGetID(SharedUtils.buildDatabaseObjectSingleField(table, id, "Id"), out output);
        }

        public static bool doDeleteByColumnGetID(string table, object info, string column, out int? output)
        {
            return doDeleteByGetID(SharedUtils.buildDatabaseObjectSingleField(table, info, column), out output);
        }
        public static bool doDeleteByGetID<TYPE>(TYPE ob, out int? output, bool includeNulls = false)
        {
            output = null;

            string declaration = "DECLARE @outputTable table(Id int NOT NULL) ";
            string extra = "OUTPUT INSERTED.Id INTO @outputTable ";
            string select = ";SELECT Id FROM @outputTable; ";

            SqlCommand cmd = new SqlCommand();
            string query = getDeleteQuery(ob, ref cmd, extra, includeNulls);
            cmd.CommandText = declaration + query + select;

            return SharedUtils.getSingleEntry(cmd, "Id", out output);
        }

        public static bool doDeleteEntryByColumn<TYPE, inT>(inT info, string column)
        {
            Type type = typeof(TYPE);
            Dictionary<string, object> conditions = new Dictionary<string, object>();
            conditions.Add(column, info);
            return doDelete(type.Name, conditions);
        }

        public static bool doDeleteEntryByColumn<inT>(string table, inT info, string column)
        {
            Dictionary<string, object> conditions = new Dictionary<string, object>();
            conditions.Add(column, info);
            return doDelete(table, conditions);
        }

        internal static string getDeleteQuery<TYPE>(TYPE ob, ref SqlCommand cmd, string preWhereExtra, bool includeNulls)
        {
            Type type = typeof(TYPE);
            string query = "";

            FieldInfo[] fields = type.GetFields();
            if (fields.Length > 0)
            {
                query = "DELETE FROM " + type.Name + " " + preWhereExtra + " WHERE ";
                for (int i = 0; i < fields.Length; i++)
                {
                    var value = SharedUtils.formatValue(fields[i].GetValue(ob));

                    if (value != null || includeNulls)
                    {

                        SqlParameter tempParam = new SqlParameter();
                        tempParam.ParameterName = "@DEL_" + Regex.Replace(fields[i].Name, "[^A-Za-z0-9 _]", "");

                        if (value is string)
                            tempParam.Value = ((string)value).Trim();
                        else tempParam.Value = value;

                        cmd.Parameters.Add(tempParam);

                        query += type.Name + "." + fields[i].Name + "=" + tempParam.ParameterName;


                        if (i + 1 < fields.Length)
                            query += " AND ";

                    }
                }
            }

            return query;
        }
        public static bool doDelete<TYPE>(TYPE ob, bool includeNulls = false)
        {
            SqlCommand cmd = new SqlCommand();
            string query = getDeleteQuery(ob, ref cmd, "", includeNulls);
            cmd.CommandText = query;
            return SharedUtils.doNonQuery(cmd);
        }

        public static bool doDelete(string table, Dictionary<string, object> values)
        {
            return doDelete(SharedUtils.buildDatabaseObject(table, values));
        }
    }
}
