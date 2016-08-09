using System;
using System.Xml.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Pokemon.Team.Builder
{
	public static class TierListSerializer
	{
		public static string SerializeTierList(List<PokemonTierEntry> tierList)
		{
			var xmlDoc = new XDocument(new XDeclaration("1.0", "utf-16", null));
			var pokemonRoot = new XElement("TierList");

			xmlDoc.Add(pokemonRoot);

			foreach (var entry in tierList)
			{
				var tierEntry = new XElement("Pokemon");

				tierEntry.Add(new XElement("MonsNo", entry.num));
				tierEntry.Add(new XElement("Tier", entry.tier));
				tierEntry.Add(new XElement("Name", entry.species));
				tierEntry.Add(new XElement("Form", entry.forme));

				pokemonRoot.Add(tierEntry);
			}

			using (var stringWriter = new StringWriter())
			{
				xmlDoc.Save(stringWriter);

				return stringWriter.ToString();
			}
		}

		public static void SaveTierListToFile(List<PokemonTierEntry> tierList, string filePath){
			File.WriteAllText(filePath, SerializeTierList(tierList), System.Text.Encoding.Unicode);
		}

		public static List<PokemonTierEntry> LoadTierListFromFile(string filePath){
			return DeserializeTierList (filePath);
		}

		public static List<PokemonTierEntry> DeserializeTierList(string filePath)
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
					select new PokemonTierEntry {
						tier = entry.Descendants("Tier").First().Value,
						species = entry.Descendants("Name").FirstOrDefault()?.Value,
						num = int.Parse(entry.Descendants("MonsNo").First().Value),
						forme = entry.Descendants("Form").First().Value
					})
					.ToList();

			return pokemon;
		}
	}
}

