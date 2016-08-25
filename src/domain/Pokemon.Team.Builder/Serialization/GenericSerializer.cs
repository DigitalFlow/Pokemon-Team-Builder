using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml;

namespace Pokemon.Team.Builder
{
    public static class GenericSerializer<T>
    {
        public static string Serialize(T values)
        {
            var serializer = new XmlSerializer(typeof(T));
            
            using (var stringWriter = new StringWriter())
            {
				var settings = new XmlWriterSettings 
				{
					CheckCharacters = true,
					Encoding = Encoding.Unicode,
					NewLineHandling = NewLineHandling.Entitize,
					Indent = true
				};

				using (var writer = XmlWriter.Create(stringWriter, settings))  {
					serializer.Serialize (writer, values);

					return stringWriter.ToString ();
				}
            }
        }

		public static void SaveToFile(T values, string filePath){
			File.WriteAllText(filePath, Serialize(values), Encoding.Unicode);
		}

		public static T LoadFromFile(string filePath){
			return Deserialize (filePath);
		}

		public static T Deserialize(string filePath)
		{
			var file = new FileInfo(filePath);

			if (!file.Exists) {
				return default(T);
			}

			using (var fileStream = file.OpenRead())
            {
                var serializer = new XmlSerializer(typeof(T));

				// Use this to not fail on invalid characters
				var xmlReader = new XmlTextReader (fileStream) {
					Normalization = true
				};

				return (T) serializer.Deserialize(xmlReader);
            }		
		}
    }
}
