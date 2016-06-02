using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;

namespace OneScanWebApp.Database
{
    public class SQLControls
    {

        public static bool getColumnByData(object checkInfo, string table, string inColumn, string outColumn, out object output)
        {
            return getSingleEntry("SELECT * FROM " + table + " WHERE " + inColumn + " = '" + checkInfo + "'", outColumn, out output);
        }

        public static bool getEntryByColumn<TYPE>(object info, string column, out List<TYPE> output)
        {
            Type type = typeof(TYPE);
            output = getData<TYPE>("SELECT * FROM " + type.Name + " WHERE " + column + " = '" + info + "'");
            return (output.Count > 0);
        }

        private static bool doNonQuery(string sql)
        {
            bool success = false;

            using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.Database.ToString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);

                if (cmd.ExecuteNonQuery() != 0)
                    success = true;
            }

            return success;

        }

        private static bool getSingleEntry(string sql, string columnName, out object output)
        {
            DataTableReader reader = getDataReader(sql);

            bool success = false;

            output = null;

            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    if (reader.GetName(i).Equals(columnName))
                    {
                        output = reader[i];
                        success = true;
                        
                    }
                }
            }


            return success;

        }

        private static List<TYPE> getData<TYPE>(string sql)
        {

            DataTableReader reader = getDataReader(sql);

            List<TYPE> returnList = new List<TYPE>();

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

        private static DataTableReader getDataReader(string sql)
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
    }
}