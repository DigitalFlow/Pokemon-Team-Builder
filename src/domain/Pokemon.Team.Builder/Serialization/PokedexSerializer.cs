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
    }
}
