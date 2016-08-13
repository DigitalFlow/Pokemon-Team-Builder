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
    public static class PokedexSerializer
    {
        public static string SerializePokedex(Pokedex pokedex)
        {
            var serializer = new XmlSerializer(typeof(Pokedex));
            
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
					serializer.Serialize (writer, pokedex);

					return stringWriter.ToString ();
				}
            }
        }

		public static void SavePokedexToFile(Pokedex pokedex, string filePath){
			File.WriteAllText(filePath, SerializePokedex(pokedex), Encoding.Unicode);
		}

		public static Pokedex LoadPokedexFromFile(string filePath){
			return DeserializePokedex (filePath);
		}

		public static Pokedex DeserializePokedex(string filePath)
		{
			var file = new FileInfo(filePath);

			if (!file.Exists) {
				return null;
			}

			using (var fileStream = file.OpenRead())
            {
                var serializer = new XmlSerializer(typeof(Pokedex));

				// Use this to not fail on invalid characters
				var xmlReader = new XmlTextReader (fileStream) {
					Normalization = true
				};

				return (Pokedex) serializer.Deserialize(xmlReader);
            }		
		}
    }
}
