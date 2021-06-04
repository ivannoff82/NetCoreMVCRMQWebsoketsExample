using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NetCoreMvcDIExample.Helpers
{
    /// <summary>
    /// Serialize/Deserialize
    /// </summary>
    public static class XMLHelper
    {
        /// <summary>
        /// Serialize object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataObject"></param>
        /// <returns></returns>
        public static string Serialize<T>(T dataObject)
        {
            var serializer = new XmlSerializer(typeof(T));
            try {               
                using (var writer = new StringWriter())
                {
                    serializer.Serialize(writer, dataObject);
                    return writer.ToString();
                }
            }
            catch (Exception ex)
            {
                //TODO: LOG error
                return null;
            }
        }

        public static T Deserialize<T>(string xml)
            {
            var serializer = new XmlSerializer(typeof(T));
            try
            {               
                using (var reader = new StringReader(xml))
                {
                    return (T)serializer.Deserialize(reader);                    
                }
            }
            catch (Exception ex)
            {
                //TODO: LOG error
                return default(T);
            }
        }



}
}
