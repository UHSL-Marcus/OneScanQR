using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SQLControls
{
    public class Get
    {
        public static bool getEntryByColumn<TYPE>(object info, string column, out List<TYPE> output)
        {
            Type type = typeof(TYPE);
            output = SharedUtils.getData<TYPE>("SELECT * FROM " + type.Name + " WHERE " + column + " = '" + info + "'");
            return (output.Count > 0);
        }

        public static bool getSingleColumnByColumn<outT, inT>(inT checkInfo, string table, string inColumn, string outColumn, out outT output)
        {
            return SharedUtils.getSingleEntry("SELECT * FROM " + table + " WHERE " + inColumn + " = '" + checkInfo + "'", outColumn, out output);
        }

        public static bool getEntryByColumn<TYPE, inT>(inT info, string column, out List<TYPE> output)
        {
            Type type = typeof(TYPE);
            output = SharedUtils.getData<TYPE>("SELECT * FROM " + type.Name + " WHERE " + column + " = '" + info + "'");
            return (output.Count > 0);
        }

        public static bool getEntryIDByColumn<TYPE, inT>(inT info, string column, out int? output)
        {
            Type type = typeof(TYPE);
            return SharedUtils.getSingleEntry("SELECT Id FROM " + type.Name + " WHERE " + column + " = '" + info + "'", "Id", out output);
        }

        public static bool getEntryExistsByColumn<TYPE, inT>(inT info, string column)
        {
            List<TYPE> l = new List<TYPE>();
            return getEntryByColumn(info, column, out l);
        }

        public static bool getEntryExists<TYPE>(TYPE ob)
        {
            int? i;
            return getEntryID(ob, out i);
        }

        public static bool getEntryID<TYPE>(TYPE ob, out int? output)
        {
            Type type = typeof(TYPE);
            output = null;

            string query = "SELECT* FROM " + type.Name + " WHERE ";

            FieldInfo[] fields = type.GetFields();
            for (int i = 0; i < fields.Length; i++)
            {

                if (!fields[i].Name.Equals("Id"))
                {
                    Type valueType = fields[i].GetType();
                    object value = fields[i].GetValue(ob);

                    query += fields[i].Name + "=" + "'" + SharedUtils.formatValue(value) + "'";

                    if (i + 1 < fields.Length)
                        query += " AND ";
                }
            }

            return SharedUtils.getSingleEntry(query, "Id", out output);
            
        }


    }
}
