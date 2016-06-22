using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SQLControls
{
    public static class Extensions
    {
        public static void ChangeFieldEquality<T>(this object ob, SQLEqualityOperations equality, [CallerMemberName]string name = "") where T : DatabaseTableObject
        {
            Type type = typeof(T);
            FieldInfo field = type.GetField(name);
            DatabaseColumnAttribute[] attrs = field.GetCustomAttributes(typeof(DatabaseColumnAttribute), false) as DatabaseColumnAttribute[];
            if (attrs.Length > 0)
            {
                attrs[0].equality = equality;
            }

        }

        public static void ChangeFieldForceUse<T>(this object ob, bool forceUse, [CallerMemberName]string name = "") where T : DatabaseTableObject
        {
            Type type = typeof(T);
            FieldInfo field = type.GetField(name);
            DatabaseColumnAttribute[] attrs = field.GetCustomAttributes(typeof(DatabaseColumnAttribute), false) as DatabaseColumnAttribute[];
            if (attrs.Length > 0)
            {
                attrs[0].forceUse = forceUse;
            }
        }
    }
}
