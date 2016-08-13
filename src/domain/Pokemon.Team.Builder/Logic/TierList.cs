using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Xml.Serialization;

namespace Pokemon.Team.Builder
{
    [Serializable]
    [XmlRoot("TierList")]
	public class TierList : IEnumerable<PokemonTierEntry>
	{
		public List<PokemonTierEntry> Pokemon;

        public TierList ()
        {
            Pokemon = new List<PokemonTierEntry> ();
        }

		public TierList (List<PokemonTierEntry> tierList)
		{
            Pokemon = tierList;
		}

        public void Add (object o)
        {
            Pokemon.Add(o as PokemonTierEntry);
        }

		IEnumerator<PokemonTierEntry> IEnumerable<PokemonTierEntry>.GetEnumerator() 
		{
			return Pokemon.GetEnumerator ();
		}

		IEnumerator IEnumerable.GetEnumerator() 
		{
			return Pokemon.GetEnumerator ();
		}

		public PokemonTierEntry GetByName(string name) 
		{
			return Pokemon.SingleOrDefault (poke => poke.species.Equals (name, StringComparison.InvariantCultureIgnoreCase));
		}

		public PokemonTierEntry GetById(int id, string formNo) 
		{
			var tierEntries = Pokemon.Where (poke => poke.num == id);

			var mega = tierEntries.FirstOrDefault(tier => tier.forme.Contains("Mega"));

			return mega ?? tierEntries.FirstOrDefault ();
		}
	}
}

