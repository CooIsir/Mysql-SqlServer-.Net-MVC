using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace com.Utility
{
    public class CXmlSerializer<T> where T : new()
    {
        private static XmlSerializer _Serializer = new XmlSerializer(typeof(T));

        public static string Serialize(T t)
        {
            string s = "";
            using (MemoryStream ms = new MemoryStream())
            {
                _Serializer.Serialize(ms, t);
                s = System.Text.UTF8Encoding.UTF8.GetString(ms.ToArray());
            }
            return s;
        }

        public static T Deserialize(string s)
        {
            T t;
            using (MemoryStream ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(s)))
            {
                t = (T)_Serializer.Deserialize(ms);
            }
            return t;
        }
    }
}
