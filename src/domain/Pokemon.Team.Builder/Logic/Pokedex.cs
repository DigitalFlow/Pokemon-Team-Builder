using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.Xml.Serialization;

namespace Pokemon.Team.Builder
{
    [Serializable]
    [XmlRoot("Pokedex")]
	[XmlInclude(typeof(Pokemon))]
	public class Pokedex : IEnumerable<Pokemon>
	{
		public List<Pokemon> Pokemon;

        public Pokedex ()
        {
            Pokemon = new List<Pokemon>();
        }

		public Pokedex (List<Pokemon> pokemon)
		{
			Pokemon = pokemon;
		}

        public void Add(object o)
        {
            Pokemon.Add(o as Pokemon);
        }

		IEnumerator<Pokemon> IEnumerable<Pokemon>.GetEnumerator() 
		{
			return Pokemon.GetEnumerator ();
		}

		IEnumerator IEnumerable.GetEnumerator() 
		{
			return Pokemon.GetEnumerator ();
		}

        /// <summary>
        /// Gets a pokemon by its name, all languages are searched for the name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
		public Pokemon GetByName(string name) 
		{
			var pokemon = Pokemon.SingleOrDefault (poke => poke.Names.Any(p => p.name.Equals (name, StringComparison.InvariantCultureIgnoreCase)));
            var varietyPokemon = Pokemon.SingleOrDefault(poke => poke.Varieties.Any(p => p.pokemon.name.Equals(name, StringComparison.InvariantCultureIgnoreCase)));
            
            if (varietyPokemon != null && varietyPokemon.Varieties.Count > 1)
            {
                var variety = varietyPokemon.Varieties.FirstOrDefault(v => v.pokemon.name.Equals(name, StringComparison.InvariantCultureIgnoreCase));

                varietyPokemon.FormNo = varietyPokemon.Varieties.IndexOf(variety).ToString();
            }

            if (pokemon == null && varietyPokemon == null)
            {
                var firstPart = name.Substring(0, name.IndexOf('-'));

                varietyPokemon = Pokemon.SingleOrDefault(poke => poke.Varieties.Any(p => p.pokemon.name.Equals(firstPart, StringComparison.InvariantCultureIgnoreCase)));
            }

            return pokemon ?? varietyPokemon;
		}

        public List<FlavorTextEntry> GetPokedexDescriptions(Pokemon pokemon, string languageCode)
        {
            return pokemon.TextEntries.Where(text => text.language != null && text.language.name == languageCode)
                .ToList();
        }

		public Pokemon GetById(int id) 
		{
			return Pokemon.SingleOrDefault (poke => poke.Id == id);
		}

        public Pokemon GetByIdentifier(PokemonIdentifier identifier)
        {
            var pokemon = GetById(identifier.MonsNo);

            pokemon.FormNo = identifier.FormNo;

            return pokemon;
        }

		public List<string> GetAvailableLanguages() 
		{
			return Pokemon.FirstOrDefault ()?.Names?.Select (name => name.language?.name).ToList();
		}
	}
}

