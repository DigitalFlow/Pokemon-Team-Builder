using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Pokemon.Team.Builder
{
    public static class PokedexSerializer
    {
        public static string SerializePokedex(Pokedex pokedex)
        {
            var serializer = new XmlSerializer(typeof(Pokedex));
            
            using (var stringWriter = new StringWriter())
            {
                serializer.Serialize(stringWriter, pokedex);

                return stringWriter.ToString();
            }
        }

		public static void SavePokedexToFile(Pokedex pokedex, string filePath){
			File.WriteAllText(filePath, SerializePokedex(pokedex), System.Text.Encoding.Unicode);
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
                return (Pokedex) serializer.Deserialize(fileStream);
            }		
		}
    }
}
