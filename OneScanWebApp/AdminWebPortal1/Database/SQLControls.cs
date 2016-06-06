using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;

namespace AdminWebPortal.Database
{
    public class SQLControls
    {

        public static bool getSingleColumnByColumn(object checkInfo, string table, string inColumn, string outColumn, out object output)
        {
            return getSingleEntry("SELECT * FROM " + table + " WHERE " + inColumn + " = '" + checkInfo + "'", outColumn, out output);
        }

        public static bool getEntryByColumn<TYPE>(object info, string column, out List<TYPE> output)
        {
            Type type = typeof(TYPE);
            output = getData<TYPE>("SELECT * FROM " + type.Name + " WHERE " + column + " = '" + info + "'");
            return (output.Count > 0);
        }

        public static bool getEntryIDByColumn<TYPE>(object info, string column, out int? output)
        {
            Type type = typeof(TYPE);
            output = null;
            bool success = false;

            object id;
            if (getSingleEntry("SELECT Id FROM " + type.Name + " WHERE " + column + " = '" + info + "'", "Id", out id))
            {
                if (id is int)
                {
                    output = (int)id;
                    success = true; ;
                }
            }
            return success;
        }

        public static bool getEntryExistsByColumn<TYPE>(object info, string column)
        {
            List<TYPE> l = new List<TYPE>();
            return getEntryByColumn<TYPE>(info, column, out l);
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

        private static object formatValue(object value)
        {
            if (value is DateTime)
                return ((DateTime)value).ToString("yyyy-MM-dd");
            else if (value is int || value is string)
                return value;
            else if (value is string[])
                return string.Join(",", value as string[]);

            return null;
        }

        public static bool getEntryExists<TYPE>(TYPE ob)
        {
            int? i;
            return getEntryID(ob, out i);
        }


        public static bool getEntryID<TYPE>(TYPE ob, out int? output)
        {
            Type type = typeof(TYPE);
            bool success = false;
            output = null;

            string query = "SELECT* FROM " + type.Name + " WHERE ";

            FieldInfo[] fields = type.GetFields();
            for (int i = 0; i < fields.Length; i++)
            {

                if (!fields[i].Name.Equals("Id"))
                {
                    Type valueType = fields[i].GetType();
                    object value = fields[i].GetValue(ob);

                    query += fields[i].Name + "=" + "'" + formatValue(value) + "'";

                    if (i + 1 < fields.Length)
                        query += " AND ";
                }
            }

            object id;
            if (getSingleEntry(query, "Id", out id)) {
                if (id is int)
                {
                    output = (int)id;
                    success = true; ;
                }
            }
            return success;
        }

        public static bool doInsertReturnID<TYPE>(TYPE ob, out int? output)
        {
            Type type = typeof(TYPE);
            output = null;
            bool success = false;

            string declaration = "DECLARE @outputTable table(Id int NOT NULL)";

            string queryName = " INSERT INTO " + type.Name;
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
                    object value = fields[i].GetValue(ob);

                    queryValues += "'" + formatValue(value) + "'";

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

            queryName += " OUTPUT INSERTED.Id INTO @outputTable";

            string select = "; SELECT Id FROM @outputTable;";

            object id;
            if(getSingleEntry(declaration + queryName + queryValues + select, "Id", out id))
            {
                if (id is int)
                {
                    output = (int)id;
                    success = true;
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