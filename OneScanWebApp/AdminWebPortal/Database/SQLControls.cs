using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;

namespace AdminWebPortal.Database
{
    public class SQLControls
    {
        public static bool doInsert(string table, Dictionary<string, object> values)
        {
            string query = "INSERT INTO " + table + " (";// Door.DoorID, Door.DoorSecret) VALUES('" + newDoorIdTxtBx.Text + "','" + newDoorSecretTxtBx + "')";
            string queryValues = " VALUES(";

            SqlCommand cmd = new SqlCommand();

            for (int i = 0; i < values.Count; i++)
            {
                string key = values.Keys.ElementAt(i);

                query += key;
                object value = values[key];

                SqlParameter tempParam = new SqlParameter();
                tempParam.ParameterName = "@" + Regex.Replace(key, "[^A-Za-z0-9 _]", "");

                if (value is string)
                    tempParam.Value = ((string)value).Trim();
                else tempParam.Value = value;

                cmd.Parameters.Add(tempParam);

                queryValues += tempParam.ParameterName;
                

                if (i < values.Count-1)
                {
                    query += ",";
                    queryValues += ",";
                }
            }

            query += ")";
            queryValues += ")";

            cmd.CommandText = query + queryValues;

            return doNonQuery(cmd);
        }

        public static bool doNonQuery(string sql)
        {
            SqlCommand cmd = new SqlCommand(sql);
            return doNonQuery(cmd);

        }

        public static bool doNonQuery(SqlCommand cmd)
        {
            bool success = false;

            using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.Database.ToString()))
            {
                conn.Open();
                cmd.Connection = conn;

                if (cmd.ExecuteNonQuery() != 0)
                    success = true;
            }

            return success;
        }


        private static List<TYPE> getData<TYPE>(string sql = null)
        {

            DataTableReader reader = getDataReader(sql);

            List<TYPE> returnList = new List<TYPE>();

            if (sql == null)



            while (reader.Read())
            {

                TYPE ob = Activator.CreateInstance<TYPE>();

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

        public static DataTableReader getDataReader(string sql)
        {
            DataSet dataSet = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter();

            using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.Database.ToString()))
            {

                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                adapter.SelectCommand = cmd;


                conn.Open();
                adapter.Fill(dataSet);

            }

            return dataSet.CreateDataReader();
        }

        public class SQLIgnoreAttribute : Attribute
        {
        }
    }
}