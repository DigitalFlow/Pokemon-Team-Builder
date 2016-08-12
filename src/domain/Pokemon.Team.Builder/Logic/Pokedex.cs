using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

namespace Pokemon.Team.Builder
{
	public class Pokedex : IEnumerable<Pokemon>
	{
		private List<Pokemon> _pokemon;

		public Pokedex (List<Pokemon> pokemon)
		{
			_pokemon = pokemon;
		}

		IEnumerator<Pokemon> IEnumerable<Pokemon>.GetEnumerator() 
		{
			return _pokemon.GetEnumerator ();
		}

		IEnumerator IEnumerable.GetEnumerator() 
		{
			return _pokemon.GetEnumerator ();
		}

		public Pokemon GetByName(string name) 
		{
			return _pokemon.SingleOrDefault (poke => poke.Names.Any(p => p.name.Equals (name, StringComparison.InvariantCultureIgnoreCase)));
		}

		public Pokemon GetById(int id) 
		{
			return _pokemon.SingleOrDefault (poke => poke.Id == id);
		}
	}
}

