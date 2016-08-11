using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace Pokemon.Team.Builder
{
	public class TierHierarchy : IEnumerable<Tier>
	{
		private List<Tier> _tiers;

		public TierHierarchy (List<Tier> tiers)
		{
			_tiers = tiers;
		}

		IEnumerator<Tier> IEnumerable<Tier>.GetEnumerator() 
		{
			return _tiers.GetEnumerator ();
		}

		IEnumerator IEnumerable.GetEnumerator() 
		{
			return _tiers.GetEnumerator ();
		}

		public List<string> GetAllWithSubTiers()
		{
			return GetAll (_tiers);
		}

		private List<string> GetAll(List<Tier> tiers)
		{
			var names = new List<string> ();

			if (tiers == null || tiers.Count == 0) {
				return names;
			}

			foreach (var tier in tiers) {
				names.Add (tier.ShortName);

				names.AddRange (GetAll (tier.SubTiers));
			}

			return names;
		}

		public Tier GetByShortName(string shortName)
		{
			return GetByShortNameInternal (shortName, _tiers).SingleOrDefault ();
		}

		private List<Tier> GetByShortNameInternal(string shortName, List<Tier> tiers)
		{
			var foundTiers = new List<Tier> ();

			if (tiers == null || tiers.Count == 0) {
				return foundTiers;
			}

			foreach (var tier in tiers) {
				if (tier.ShortName.Equals (shortName, StringComparison.InvariantCultureIgnoreCase)) 
				{
					foundTiers.Add(tier);
				}

				foundTiers.AddRange(GetByShortNameInternal (shortName, tier.SubTiers));
			}

			return foundTiers;
		}
	}
}

