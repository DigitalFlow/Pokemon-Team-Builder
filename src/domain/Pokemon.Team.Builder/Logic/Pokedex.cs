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
			return Pokemon.SingleOrDefault (poke => poke.Names.Any(p => p.name.Equals (name, StringComparison.InvariantCultureIgnoreCase)));
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

		public List<string> GetAvailableLanguages() 
		{
			return Pokemon.FirstOrDefault ()?.Names?.Select (name => name.language?.name).ToList();
		}
	}
}

