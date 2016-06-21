﻿using SQLControls;
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

        public static bool doSelectEntryExists<TYPE>(DatabaseTableObject ob, bool includeNulls = false)
        {
            int? i;
            return doSelectID<TYPE>(ob, out i, includeNulls);
        }
        public static bool doSelectID<TYPE>(DatabaseTableObject ob, out int? output, bool includeNulls = false)
        {
            return doSelectSingleColumn<TYPE, int?>(ob, "Id", out output, includeNulls);
        }

        public static bool doSelectSingleColumn<TYPE, outT>(DatabaseTableObject ob, string column, out outT output, bool includeNulls = false)
        {
            SqlCommand cmd = new SqlCommand();
            string query = getSelectQuery(ob, ref cmd, column, "", includeNulls);
            cmd.CommandText = query;
            return SharedUtils.getSingleEntry(cmd, column, out output);
        }

        public static bool doSelectAllSingleColumn<TYPE, outType>(DatabaseTableObject ob, string selArg, string columnName, out List<outType> output, bool includeNulls = false)
        {
            SqlCommand cmd = new SqlCommand();
            string query = getSelectQuery(ob, ref cmd, columnName, "", includeNulls);
            cmd.CommandText = query;
            output = SharedUtils.getData<outType>(cmd, columnName);
            return (output.Count > 0);

        }

        public static bool doSelect<TYPE>(DatabaseTableObject ob, string selArg, out List<TYPE> output, bool includeNulls = false)
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

        public static bool doJoinSelect<TYPE>(DatabaseTableObject where, out List<TYPE> output, JoinPair[] joins, bool includeNulls = false) where TYPE:IDatabaseOutputObject
        {
            return doJoinSelect(new Tuple<DatabaseTableObject, string, bool>[] { Tuple.Create(where, "", includeNulls) }, out output, joins);
        }
        public static bool doJoinSelect<TYPE>(Tuple<DatabaseTableObject, string, bool>[] whereobs, out List<TYPE> output, JoinPair[] joins) where TYPE : IDatabaseOutputObject
        {
            SqlCommand cmd = new SqlCommand();
            string query = getJoinSelectQuery(ref cmd, getSelArgument<TYPE>(), whereobs, joins);
            cmd.CommandText = query;
            output = SharedUtils.getData<TYPE>(cmd);
            return (output.Count > 0);
        }

        public static bool doJoinSelect(DatabaseTableObject where, string selArg, out List<Dictionary<string, object>> output, JoinPair[] joins, bool includeNulls = false)
        {
            return doJoinSelect(new Tuple<DatabaseTableObject, string, bool>[] { Tuple.Create(where, "", includeNulls) }, selArg, out output, joins);
        }
        public static bool doJoinSelect(Tuple<DatabaseTableObject, string, bool>[] whereobs, string selArg, out List<Dictionary<string, object>> output, JoinPair[] joins)
        {
            SqlCommand cmd = new SqlCommand();
            string query = getJoinSelectQuery(ref cmd, selArg, whereobs, joins);
            cmd.CommandText = query;
            output = SharedUtils.getData(cmd);
            return (output.Count > 0);
        }

        internal static string getSelArgument<TYPE>() where TYPE:IDatabaseOutputObject
        {
            Type type = typeof(TYPE);
            string selectArgs = "";

            FieldInfo[] fields = type.GetFields();
            if (fields.Length > 0)
            {
                for (int i = 0; i < fields.Length; i++)
                {
                    string columnName = "";

                    Attribute[] attributes = Attribute.GetCustomAttributes(fields[i]);
                    foreach (Attribute attribute in attributes)
                    {
                        if (attribute is DatabaseOutputAttribute)
                        {
                            DatabaseOutputAttribute attr = attribute as DatabaseOutputAttribute;
                            if (!attr.SQLIgnore)
                            {
                                columnName = fields[i].Name;
                                if (attr.columnName != null)
                                {
                                    columnName = attr.columnName + columnName;
                                }
                            }

                        }
                    }

                    if (i < fields.Length - 1)
                        columnName += ",";

                    selectArgs += columnName;

                }
            }

            return selectArgs;
        }

        private delegate bool CanAddJoin(JoinPair join); 
        internal static string getJoinSelectQuery(ref SqlCommand cmd, string selectArg, Tuple<DatabaseTableObject, string, bool>[] whereObs, JoinPair[] joins)
        {
            List<JoinPair> done = new List<JoinPair>();

            string joinString = buildJoin(joins[0]);
            done.Add(joins[0]);

            CanAddJoin canAddJoin = null;
            canAddJoin = delegate (JoinPair join)
            {
                bool canAdd = false;

                if (done.Contains(join)) return true;

                foreach (JoinPair doneJoin in done)
                {
                    if ((join.leftTable.Name.Equals(doneJoin.leftTable.Name) || join.leftTable.Name.Equals(doneJoin.rightTable.Name)) &&
                        (!join.rightTable.Name.Equals(doneJoin.leftTable.Name) && !join.rightTable.Name.Equals(doneJoin.rightTable.Name)))
                    {
                        canAdd = true;
                        break;
                    }
                    else
                    {
                        foreach (JoinPair todoJoin in joins)
                        {
                            if (join.leftTable.Name.Equals(todoJoin.rightTable.Name))
                            {
                                canAdd = canAddJoin(todoJoin);
                                break;
                            }
                        }
                    }
                }

                if (canAdd)
                {
                    joinString += buildJoin(join);
                    done.Add(joins[0]);
                }

                return canAdd;
            };

            for (int i = 1; i < joins.Length; i++)
            {
                canAddJoin(joins[i]);
            }

            return getSelectQuery(whereObs, joins[0].leftTable.Name, ref cmd, selectArg, joinString);
        }

        private static string buildJoin(JoinPair join)
        {
            string joinString = " JOIN " + join.rightTable.GetType().Name + " ON ";
            JoinOnPair[] ons = join.ons;
            for(int i = 0; i < ons.Length; i++)
            {
                if (i != 0)
                    joinString += ons[i].conjunc;

                joinString += join.leftTable.GetType().Name + "." + ons[i].leftTableCol + ons[i].op + join.rightTable.GetType().Name + "." + ons[i].rightTableCol + " ";
            }
            return joinString;
        }


        internal static string getSelectQuery(Tuple<DatabaseTableObject, string, bool>[] obs, string table, ref SqlCommand cmd, string selectArg, string preWhereExtra)
        {
            
            return "SELECT " + selectArg + " FROM " + table + " " + preWhereExtra + " WHERE " +
                    getWhere(obs, ref cmd); 
        }

        private static string getWhere(Tuple<DatabaseTableObject, string, bool>[] obs, ref SqlCommand cmd)
        {
            string query = "";

            for(int t = 0; t < obs.Length; t++)
            {
                Tuple<DatabaseTableObject, string, bool> ob = obs[t];
                Type type = ob.GetType();

                FieldInfo[] fields = type.GetFields();
                if (fields.Length > 0)
                {
                    if (t > 0)
                        query += " " + ob.Item2 + " ";

                        query += "(";
                    for (int i = 0; i < fields.Length; i++)
                    {

                        var value = SharedUtils.formatValue(fields[i].GetValue(ob.Item1));

                        if (value != null || ob.Item3)
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
                    query += ")";
                }
            }

            return query;
        }


        internal static string getSelectQuery(DatabaseTableObject ob, ref SqlCommand cmd, string selectArg, string preWhereExtra, bool includeNulls)
        {
            return getSelectQuery(new Tuple<DatabaseTableObject, string, bool>[] { Tuple.Create(ob, "", includeNulls) }, ob.GetType().Name, ref cmd, selectArg, preWhereExtra);
        }
 
    }
}
