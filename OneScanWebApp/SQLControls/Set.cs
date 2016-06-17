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
    public class Set
    {
        public static bool doInsert<TYPE>(TYPE ob)
        {
            SqlCommand cmd = new SqlCommand();
            string query = getInsertQuery(ob, ref cmd);
            cmd.CommandText = query;
            return SharedUtils.doNonQuery(cmd);
        }

        public static bool doInsertReturnID<TYPE>(TYPE ob, out int? output)
        {
            output = null;
            Type type = typeof(TYPE);

            string declaration = "DECLARE @outputTable table(Id int NOT NULL) ";
            string outputExtra = " OUTPUT INSERTED.Id INTO @outputTable";
            string select = "; SELECT Id FROM @outputTable;";

            SqlCommand cmd = new SqlCommand();
            string query = getInsertQuery(ob, ref cmd, outputExtra, select);
            cmd.CommandText = declaration + query;


            return SharedUtils.getSingleEntry(cmd, "Id", out output);

        }

        internal static string getInsertQuery<TYPE>(TYPE ob, ref SqlCommand cmd, string queryNameExtra = "", string queryValuesExtra = "")
        {
            Type type = typeof(TYPE);
            string queryName = "INSERT INTO " + type.Name;
            string queryValues = "";

            FieldInfo[] fields = type.GetFields();
            for (int i = 0; i < fields.Length; i++)
            {

                if (!fields[i].Name.Equals("Id"))
                {
                    if (queryValues.Length < 1)
                    {
                        queryName += "(";
                        queryValues += " VALUES(";
                    }

                    queryName += fields[i].Name;

                    Type valueType = fields[i].GetType();
                    var value = SharedUtils.formatValue(fields[i].GetValue(ob));

                    SqlParameter tempParam = new SqlParameter();
                    tempParam.ParameterName = "@INS_" + Regex.Replace(fields[i].Name, "[^A-Za-z0-9 _]", "");

                    if (value is string)
                        tempParam.Value = ((string)value).Trim();
                    else tempParam.Value = value;

                    cmd.Parameters.Add(tempParam);

                    queryValues += tempParam.ParameterName;


                    if (i + 1 < fields.Length)
                    {
                        queryName += ",";
                        queryValues += ",";
                    }
                }
            }

            if (queryValues.Length > 0)
            {
                queryName += ")";
                queryValues += ")";
            }
            else queryValues += " DEFAULT VALUES";

            return queryName + queryNameExtra + queryValues + queryValuesExtra;
        }

        public static bool doInsert(string table, Dictionary<string, object> values)
        {
            return doInsert(SharedUtils.buildDatabaseObject(table, values));
        }
    }
}
