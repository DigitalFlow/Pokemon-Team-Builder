using System;
using System.Collections.Generic;
using System.Linq;

namespace Pokemon.Team.Builder
{
	public class PokemonTierEntry
	{
		public class GenderRatio
		{
			public double M { get; set; }
			public double F { get; set; }
		}

		public class BaseStats
		{
			public int hp { get; set; }
			public int atk { get; set; }
			public int def { get; set; }
			public int spa { get; set; }
			public int spd { get; set; }
			public int spe { get; set; }
		}

		public class Abilities
		{
			public string _0 { get; set; }
			public string _1 { get; set; }
			public string H { get; set; }
		}

		public int num { get; set; }
		public string species { get; set; }
		public List<string> types { get; set; }
		public GenderRatio genderRatio { get; set; }
		public BaseStats baseStats { get; set; }
		public Abilities abilities { get; set; }
		public double heightm { get; set; }
		public double weightkg { get; set; }
		public string color { get; set; }
		public List<string> evos { get; set; }
		public List<string> eggGroups { get; set; }
		public string tier { get; set; }
		public string prevo { get; set; }
		public int? evoLevel { get; set; }
		public List<string> otherFormes { get; set; }
		public string baseSpecies { get; set; }
		public string forme { get; set; }
		public string formeLetter { get; set; }
		public string requiredItem { get; set; }

		public bool IsInTierOrBelow (Tier tier) {
			return this.tier.Equals (tier.ShortName, StringComparison.InvariantCultureIgnoreCase)
				|| (tier.SubTiers != null && tier.SubTiers.Any (t => IsInTierOrBelow(t)));
		}
	}
}

