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
					// TODO: Fix issues with text escapers (such as \n) from response that break xml and check characters
					CheckCharacters = false,
					Encoding = Encoding.Unicode,
					NewLineHandling = NewLineHandling.Entitize,
					Indent = true
				};

				using (var writer = XmlTextWriter.Create (stringWriter, settings))  {
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
					// TODO: Enable normalization once text escaper issues are fixed
					Normalization = false
				};

				return (Pokedex) serializer.Deserialize(xmlReader);
            }		
		}
    }
}
