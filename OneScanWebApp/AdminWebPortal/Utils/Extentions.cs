using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Xml;
using System.Xml.Serialization;

namespace AdminWebPortal.Utils
{
    public static class Extentions
    {
        public static string SerializeObject<T>(this T toSerialize, bool base64 = false, bool Json = false)
        {
            StringBuilder builder = new StringBuilder();
            if (!Json)
            {
                using (XmlTextWriter textWriter = new XmlTextWriter(new StringWriter(builder)))
                {
                    bool dataContractAttr = Attribute.IsDefined(typeof(T), typeof(DataContractAttribute));


                    if (dataContractAttr)
                    {
                        DataContractSerializer serializer = new DataContractSerializer(toSerialize.GetType());
                        serializer.WriteObject(textWriter, toSerialize);
                    }
                    else
                    {
                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                        xmlSerializer.Serialize(textWriter, toSerialize);
                    }
                }
            }
            else
            {
                builder.Append(JsonUtils.GetJson(toSerialize));
            }

            string output = builder.ToString();
            if (base64) output = Convert.ToBase64String(Encoding.Unicode.GetBytes(output));
            return output;
            
        }

        public static bool TryDeserializeObject<T>(this string toDeserialize, out T output, bool base64 = false, bool Json = false)
        {
            bool success = false;
            output = Activator.CreateInstance<T>();

            try
            {
                if (base64) toDeserialize = Encoding.Unicode.GetString(Convert.FromBase64String(toDeserialize));

                if (!Json)
                {

                    using (XmlTextReader textReader = new XmlTextReader(new StringReader(toDeserialize)))
                    {
                        bool dataContractAttr = Attribute.IsDefined(typeof(T), typeof(DataContractAttribute));
                        if (dataContractAttr)
                        {
                            DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                            output = (T)serializer.ReadObject(textReader);
                        }
                        else
                        {
                            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                            output = (T)xmlSerializer.Deserialize(textReader);
                        }
                        success = true;
                    }
                }
                else
                {
                    output = JsonUtils.GetObject<T>(toDeserialize);
                    success = true;
                }
            }
            catch (Exception e)
            {

            }
            return success;  
        }

        public static Control FindControlRecursive(this Control rootControl, string controlID)
        {
            if (rootControl.ID == controlID) return rootControl;

            foreach (Control controlToSearch in rootControl.Controls)
            {
                Control controlToReturn =
                    controlToSearch.FindControlRecursive(controlID);
                if (controlToReturn != null) return controlToReturn;
            }
            return null;
        }

        public static bool FindControlRecursive<T>(this Control rootControl, string controlID, out T control) where T : Control
        {
            bool success = true;
            control = null;

            if (rootControl.ID == controlID) control = rootControl as T;
            if (control == null)
            {
                success = false;
                foreach (Control controlToSearch in rootControl.Controls)
                {
                    success = controlToSearch.FindControlRecursive(controlID, out control);
                    if (success) break;
                }
            }
            return success;
        }

        public static bool FindParent<T>(this Control target, out T parent) where T : Control
        {
            bool success = false;
            parent = null;

            if (target.Parent != null)
            {

                parent = target.Parent as T;
                if (parent != null)
                    success = true;
                else success = target.Parent.FindParent(out parent);
            }
            return success;
        }

    }
}