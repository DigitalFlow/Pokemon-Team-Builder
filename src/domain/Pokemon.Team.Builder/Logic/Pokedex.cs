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

		public Pokemon GetByName(string name) 
		{
			return Pokemon.SingleOrDefault (poke => poke.Names.Any(p => p.name.Equals (name, StringComparison.InvariantCultureIgnoreCase)));
		}

		public Pokemon GetById(int id) 
		{
			return Pokemon.SingleOrDefault (poke => poke.Id == id);
		}
	}
}

