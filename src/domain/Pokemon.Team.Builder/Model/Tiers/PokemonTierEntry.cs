using Pokemon.Team.Builder.Model.Tiers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pokemon.Team.Builder
{
    [Serializable]
	public class PokemonTierEntry
	{
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

