using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StringEnum
{
    public class StringValueAttribute : Attribute
    {

        private string _value;

        public StringValueAttribute(string value)
        {
            _value = value;
        }

        public string Value
        {
            get { return _value; }
        }

    }
    public static class StringEnum
    {
        public static string GetStringValue(this Enum value)
        {
            string output = null;
            Type type = value.GetType();

            FieldInfo fi = type.GetField(value.ToString());
            StringValueAttribute[] attrs =
                fi.GetCustomAttributes(typeof(StringValueAttribute),
                                        false) as StringValueAttribute[];
            if (attrs.Length > 0)
                output = attrs[0].Value;
            
            return output;
        }
    }
}
