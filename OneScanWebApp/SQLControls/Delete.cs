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
        /*public static bool doDeleteByIDGetID(string id, string table, out int? output)
        {
            return doDeleteByGetID(SharedUtils.buildDatabaseObjectSingleField(table, id, "Id"), out output);
        }*/

        /*public static bool doDeleteByColumnGetID(string table, object info, string column, out int? output)
        {
            return doDeleteByGetID(SharedUtils.buildDatabaseObjectSingleField(table, info, column), out output);
        }*/

        /*public static bool doDeleteByGetID<TYPE>(TYPE ob, out int? output, bool includeNulls = false)
        {
            output = null;

            string declaration = "DECLARE @outputTable table(Id int NOT NULL) ";
            string extra = "OUTPUT INSERTED.Id INTO @outputTable ";
            string select = ";SELECT Id FROM @outputTable; ";

            SqlCommand cmd = new SqlCommand();
            string query = getDeleteQuery(ob, ref cmd, extra, includeNulls);
            cmd.CommandText = declaration + query + select;

            return SharedUtils.getSingleEntry(cmd, "Id", out output);
        }*/

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

        internal static string getDeleteQuery(Tuple<DatabaseTableObject, SQLWhereConjuctions, bool>[] obs, string table, ref SqlCommand cmd, string preWhereExtra)
        {
            return "DELETE FROM " + table + " " + preWhereExtra + " WHERE " +
                    SharedUtils.getWhere(obs, ref cmd, "DEL_");
        }

        public static bool doDelete<TYPE>(TYPE ob, bool includeNulls = false) where TYPE:DatabaseTableObject
        {
            SqlCommand cmd = new SqlCommand();
            string query = getDeleteQuery(new Tuple<DatabaseTableObject, SQLWhereConjuctions, bool>[] { Tuple.Create((DatabaseTableObject)ob, SQLWhereConjuctions.AND, includeNulls) }, typeof(TYPE).Name, ref cmd, ""); 
            cmd.CommandText = query;
            return SharedUtils.doNonQuery(cmd);
        }

        public static bool doDelete(string table, Dictionary<string, object> values)
        {
            return doDelete(SharedUtils.buildDatabaseObject(table, values));
        }
    }
}
