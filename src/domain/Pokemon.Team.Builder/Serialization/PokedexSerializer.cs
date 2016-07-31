using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Pokemon.Team.Builder
{
    public static class PokedexSerializer
    {
        public static string SerializePokedex(List<Pokemon> pokemon)
        {
            var xmlDoc = new XDocument(new XDeclaration("1.0", "utf-16", null));
            var pokemonRoot = new XElement("Pokedex");

            xmlDoc.Add(pokemonRoot);

            foreach (var entry in pokemon)
            {
                var pokedexEntry = new XElement("Pokemon");

                pokedexEntry.Add(new XElement("Id", entry.Id));
                pokedexEntry.Add(new XElement("Name", entry.Name));
                pokedexEntry.Add(new XElement("Url", entry.Url));

                pokemonRoot.Add(pokedexEntry);
            }

            using (var stringWriter = new StringWriter())
            {
                xmlDoc.Save(stringWriter);

                return stringWriter.ToString();
            }
        }

		public static void SavePokedexToFile(List<Pokemon> pokemon, string filePath){
			File.WriteAllText(filePath, PokedexSerializer.SerializePokedex(pokemon), System.Text.Encoding.Unicode);
		}

		public static List<Pokemon> LoadPokedexFromFile(string filePath){
			return DeserializePokedex (filePath);
		}

		public static List<Pokemon> DeserializePokedex(string filePath)
		{
			var file = new FileInfo(filePath);

			if (!file.Exists) {
				return null;
			}

			var fileStream = file.OpenRead ();

			var xmlDoc = XDocument.Load (fileStream);
			var nameSpace = xmlDoc.Root.Name.Namespace;

			var pokemon = 
				(from entry in xmlDoc.Descendants (nameSpace + "Pokemon")
				select new Pokemon {
					Name = entry.Descendants("Name").First().Value,
					Id = int.Parse(entry.Descendants("Id").First().Value),
					Url = entry.Descendants("Url").First().Value
				})
				.ToList();

			return pokemon;
		}
    }
}
