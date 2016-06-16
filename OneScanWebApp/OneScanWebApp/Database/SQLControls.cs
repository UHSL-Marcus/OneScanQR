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

        public static bool getSingleColumnByColumn<outT, inT>(inT checkInfo, string table, string inColumn, string outColumn, out outT output)
        {
            return getSingleEntry("SELECT * FROM " + table + " WHERE " + inColumn + " = '" + checkInfo + "'", outColumn, out output);
        }

        public static bool getEntryByColumn<TYPE, inT>(inT info, string column, out List<TYPE> output)
        {
            Type type = typeof(TYPE);
            output = getData<TYPE>("SELECT * FROM " + type.Name + " WHERE " + column + " = '" + info + "'");
            return (output.Count > 0);
        }

        public static bool getEntryIDByColumn<TYPE, inT>(inT info, string column, out int? output)
        {
            Type type = typeof(TYPE);
            return getSingleEntry("SELECT Id FROM " + type.Name + " WHERE " + column + " = '" + info + "'", "Id", out output);
        }

        public static bool deleteEntryByColumn<TYPE, inT>(inT info, string column)
        {
            Type type = typeof(TYPE);
            string query = "DELETE FROM " + type.Name + " WHERE " + column + "='" + info + "'";
            return doNonQuery(query);

        }

        public static bool getEntryExistsByColumn<TYPE, inT>(inT info, string column)
        {
            List<TYPE> l = new List<TYPE>();
            return getEntryByColumn(info, column, out l);
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

        private static bool getSingleEntry<T>(string sql, string columnName, out T output)
        {
            DataTableReader reader = getDataReader(sql);

            bool success = false;

            output = default(T);

            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    if (reader.GetName(i).Equals(columnName))
                    {
                        if (reader[i] is T)
                        {
                            output = (T)reader[i];
                            success = true;
                        }
                        
                    }
                }
            }


            return success;

        }

        private static object formatValue(object value)
        {
            if (value is DateTime)
                return ((DateTime)value).ToString("yyyy/MM/dd HH:mm:ss.fK");
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

        private static string getInsertQuery<TYPE>(TYPE ob, string queryNameExtra = "", string queryValuesExtra = "")
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

            return queryName + queryNameExtra + queryValues + queryValuesExtra;
        }

        private static string getUpdateQuery<TYPE>(TYPE ob, string[] testColumns)
        {
            Type type = typeof(TYPE);
            string query = "UPDATE " + type.Name + " SET ";
            string where = "WHERE ";

            FieldInfo[] fields = type.GetFields();
            for (int i = 0; i < fields.Length; i++)
            {
                string fName = fields[i].Name;
                if (!fName.Equals("Id"))
                {
                    string entry = fName + "='" + formatValue(fields[i].GetValue(ob)) + "'";
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

            return query + where;
        }

        public static bool doInsert<TYPE>(TYPE ob)
        {
            string query = getInsertQuery(ob);
            return doNonQuery(query);
        }

        public static bool doInsertReturnID<TYPE>(TYPE ob, out int? output)
        {
            output = null;
            Type type = typeof(TYPE);
           
            bool success = false;

            string declaration = "DECLARE @outputTable table(Id int NOT NULL) ";
            string outputExtra = " OUTPUT INSERTED.Id INTO @outputTable";
            string select = "; SELECT Id FROM @outputTable;";

            string query = getInsertQuery(ob, outputExtra, select);

            object id;
            if(getSingleEntry(declaration + query, "Id", out id))
            {
                if (id is int)
                {
                    output = (int)id;
                    success = true;
                }
            }
            return success;

        }


        public static bool doUpdateOrInsert<TYPE>(TYPE ob, string testColumn)
        {
            return doUpdateOrInsert(ob, new string[] { testColumn });
        }
        public static bool doUpdateOrInsert<TYPE>(TYPE ob, string[] testColumns)
        {
            Type type = typeof(Type);

            bool success = false;

            string query = string.Format(@" SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
                                            BEGIN TRANSACTION;
                                                {0};
                                                IF @@ROWCOUNT = 0
                                                BEGIN
                                                {1};
                                                END
                                            COMMIT TRANSACTION;", getUpdateQuery(ob, testColumns), getInsertQuery(ob));

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