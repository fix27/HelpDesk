using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace HelpDesk.Common
{
    /// <summary>
    /// Для xml-сериализации/десериализации 
    /// </summary>
    public static class TypeSerialiser<T> where T : new()
    {
        /// <summary>
        /// Сериализация типа
        /// </summary>
        /// <param name="t">Сериализуемый объект</param>
        /// <returns>xml-строка</returns>
        public static string StringSerialize(T obj)
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(T));
            
            var result = new StringBuilder();
            using (var stringWriter = new StringWriter(result))
            using (var xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings { Indent = true }))
            {
                serializer.WriteObject(xmlWriter, obj);
            }

            return result.ToString();
        }

        /// <summary>
        /// Десериализация
        /// </summary>
        /// <param name="xml">xml-строка</param>
        /// <returns>Объект</returns>
        public static T StringDeserialize(string xml)
        {
            if (string.IsNullOrWhiteSpace(xml))
                return default(T);

            DataContractSerializer serializer = new DataContractSerializer(typeof(T));

            using (var stringReader = new StringReader(xml))
            using (var xmlReader = XmlReader.Create(stringReader))
            {
                return (T)serializer.ReadObject(xmlReader);
            }
        }

    }
       
}
