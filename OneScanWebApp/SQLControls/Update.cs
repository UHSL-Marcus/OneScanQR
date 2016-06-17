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
    public class Update
    {

        internal static string getUpdateQuery<TYPE>(TYPE ob, ref SqlCommand cmd, string[] testColumns)
        {
            Type type = typeof(TYPE);
            string query = "UPDATE " + type.Name + " SET ";
            string where = " WHERE ";

            FieldInfo[] fields = type.GetFields();
            for (int i = 0; i < fields.Length; i++)
            {
                string fName = fields[i].Name;
                if (!fName.Equals("Id"))
                {
                    SqlParameter tempParam = new SqlParameter();
                    tempParam.ParameterName = "@UPD_" + Regex.Replace(fName, "[^A-Za-z0-9 _]", "");

                    var value = SharedUtils.formatValue(fields[i].GetValue(ob));

                    if (value is string)
                        tempParam.Value = ((string)value).Trim();
                    else tempParam.Value = value;

                    cmd.Parameters.Add(tempParam);

                    string entry = fName + "=" + tempParam.ParameterName;
                    query += entry;

                    if (testColumns.Contains(fName))
                        where += entry;

                    if (i + 1 < fields.Length)
                    {
                        query += ",";
                        if (testColumns.Length > 1)
                            where += " AND ";
                    }
                }
            }

            return query + (testColumns.Length > 0 ? where : "");
        }

        public static bool doUpdateOrInsert<TYPE>(TYPE ob, string testColumn)
        {
            return doUpdateOrInsert(ob, new string[] { testColumn });
        }
        public static bool doUpdateOrInsert<TYPE>(TYPE ob, string[] testColumns)
        {
            Type type = typeof(Type);

            bool success = false;
            SqlCommand cmd = new SqlCommand();
            string query = string.Format(@" SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
                                            BEGIN TRANSACTION;
                                                {0};
                                                IF @@ROWCOUNT = 0
                                                BEGIN
                                                {1};
                                                END
                                            COMMIT TRANSACTION;", getUpdateQuery(ob, ref cmd, testColumns), Set.getInsertQuery(ob, ref cmd));
            cmd.CommandText = query;
            success = SharedUtils.doNonQuery(cmd);

            return success;
        }
    }
}
