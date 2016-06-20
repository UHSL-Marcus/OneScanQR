using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SQLControls
{
    public class Get
    {
        public static bool doSelectByColumn<TYPE, inT>(inT info, string column, out List<TYPE> output)
        {
            return doSelect<TYPE>(SharedUtils.buildDatabaseObjectSingleField(typeof(TYPE).Name, info, column), "*", out output);
        }

        public static bool doSelectSingleColumnByColumn<TYPE, outT, inT>(inT checkInfo, string inColumn, string outColumn, out outT output)
        {
            return doSelectSingleColumnByColumn(checkInfo, typeof(TYPE).Name, inColumn, outColumn, out output);
        }
        public static bool doSelectSingleColumnByColumn<outT, inT>(inT checkInfo, string table, string inColumn, string outColumn, out outT output)
        {
            return doSelectSingleColumn(SharedUtils.buildDatabaseObjectSingleField(table, checkInfo, inColumn), outColumn, out output, false);
        }

        public static bool doSelectIDByColumn<TYPE, inT>(inT info, string column, out int? output)
        {
            return doSelectSingleColumnByColumn(info, typeof(TYPE).Name, column, "Id", out output);
        }

        public static bool getEntryExistsByColumn<TYPE, inT>(inT info, string column)
        {
            int? i;
            return doSelectIDByColumn<TYPE, inT>(info, column, out i);
        }

        public static bool doSelectEntryExists<TYPE>(TYPE ob, bool includeNulls = false)
        {
            int? i;
            return doSelectID(ob, out i, includeNulls);
        }
        public static bool doSelectID<TYPE>(TYPE ob, out int? output, bool includeNulls = false)
        {
            return doSelectSingleColumn(ob, "Id", out output, includeNulls);
        }

        public static bool doSelectSingleColumn<TYPE, outT>(TYPE ob, string column, out outT output, bool includeNulls = false)
        {
            SqlCommand cmd = new SqlCommand();
            string query = getSelectQuery(ob, ref cmd, column, "", includeNulls);
            cmd.CommandText = query;
            return SharedUtils.getSingleEntry(cmd, column, out output);
        }

        public static bool doSelectAllSingleColumn<TYPE, outType>(TYPE ob, string selArg, string columnName, out List<outType> output, bool includeNulls = false)
        {
            SqlCommand cmd = new SqlCommand();
            string query = getSelectQuery(ob, ref cmd, columnName, "", includeNulls);
            cmd.CommandText = query;
            output = SharedUtils.getData<outType>(cmd, columnName);
            return (output.Count > 0);

        }

        public static bool doSelect<TYPE>(TYPE ob, string selArg, out List<TYPE> output, bool includeNulls = false)
        {
            SqlCommand cmd = new SqlCommand();
            string query = getSelectQuery(ob, ref cmd, selArg, "", includeNulls);
            cmd.CommandText = query;
            output = SharedUtils.getData<TYPE>(cmd);
            return (output.Count > 0);
        }

        public static bool doSelect<TYPE>(string selArg, Dictionary<string, object> values, out List<TYPE> output)
        {
            Type type = typeof(TYPE);
            return doSelect<TYPE>(SharedUtils.buildDatabaseObject(type.Name, values), selArg, out output);
        }


        internal static string getJoinSelectQuery(ref SqlCommand cmd, string selectArg)
        {

        }


        internal static string getSelectQuery<TYPE>(TYPE ob, string table, ref SqlCommand cmd, string selectArg, string preWhereExtra, bool includeNulls)
        {
            Type type = typeof(TYPE);
            string query = "";

            FieldInfo[] fields = type.GetFields();
            if (fields.Length > 0)
            {
                query = "SELECT " + selectArg + " FROM " + table + " " + preWhereExtra + " WHERE ";
                for (int i = 0; i < fields.Length; i++)
                {

                    var value = SharedUtils.formatValue(fields[i].GetValue(ob));

                    if (value != null || includeNulls)
                    {
                        SqlParameter tempParam = new SqlParameter();
                        tempParam.ParameterName = "@SEL_" + Regex.Replace(fields[i].Name, "[^A-Za-z0-9 _]", "");

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


        internal static string getSelectQuery<TYPE>(TYPE ob, ref SqlCommand cmd, string selectArg, string preWhereExtra, bool includeNulls)
        {
            return getSelectQuery(ob, typeof(TYPE).Name, ref cmd, selectArg, preWhereExtra, includeNulls);
        }
 
    }
}
