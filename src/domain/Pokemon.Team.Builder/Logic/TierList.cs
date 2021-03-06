﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Xml.Serialization;

namespace Pokemon.Team.Builder
{
    [Serializable]
    [XmlRoot("TierList")]
	[XmlInclude(typeof(PokemonTierEntry))]
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

		public PokemonTierEntry Get(string name) 
		{
			return Pokemon.SingleOrDefault (poke => poke.species.Equals (name, StringComparison.InvariantCultureIgnoreCase));
		}

        public PokemonTierEntry Get(IPokemonIdentifiable poke)
        {
            return Get(poke.Identifier.Name) ?? Get(poke.Identifier.MonsNo, poke.Identifier.FormNo);
        }

		public PokemonTierEntry Get(int id, string formNo) 
		{
			var tierEntries = Pokemon.Where (poke => poke.num == id);
            
            var form = 0;
            int.TryParse(formNo, out form);

            if (form != 0)
            {
                var formLink = tierEntries.FirstOrDefault().otherFormes[form].Replace("-", string.Empty);
                var formEntry = Pokemon.Single(poke => poke.num == id && poke.species.Replace("-", string.Empty).Equals(formLink, StringComparison.InvariantCultureIgnoreCase));

                return formEntry;
            }

            return  tierEntries.FirstOrDefault ();
		}
	}
}

