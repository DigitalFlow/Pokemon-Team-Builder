using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace Pokemon.Team.Builder
{
	public class TierList : IEnumerable<PokemonTierEntry>
	{
		private List<PokemonTierEntry> _tierList;

		public TierList (List<PokemonTierEntry> tierList)
		{
			_tierList = tierList;
		}

		IEnumerator<PokemonTierEntry> IEnumerable<PokemonTierEntry>.GetEnumerator() 
		{
			return _tierList.GetEnumerator ();
		}

		IEnumerator IEnumerable.GetEnumerator() 
		{
			return _tierList.GetEnumerator ();
		}

		public PokemonTierEntry GetByName(string name) 
		{
			return _tierList.SingleOrDefault (poke => poke.species.Equals (name, StringComparison.InvariantCultureIgnoreCase));
		}

		public PokemonTierEntry GetById(int id) 
		{
			return _tierList.SingleOrDefault (poke => poke.num == id);
		}
	}
}

