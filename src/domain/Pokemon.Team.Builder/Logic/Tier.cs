using System;
using System.Collections.Generic;
using System.Linq;

namespace Pokemon.Team.Builder
{
	public class Tier
	{
		public string FullName { get; set; }
		public string ShortName { get; set; }
		public List<Tier> SubTiers { get; set; }

		public override bool Equals(object obj)
		{
			var otherTier = obj as Tier;

			if(otherTier == null)
			{
				return false;
			}

			if(SubTiers.Count != otherTier.SubTiers.Count)
			{
				return false;
			}

			var tuples = new List<Tuple<Tier, Tier>>();

			for (var i = 0; i < SubTiers.Count; i++)
			{
				tuples.Add(Tuple.Create(SubTiers[i], otherTier.SubTiers[i]));   
			}

			return FullName == otherTier.FullName
				&& ShortName == otherTier.ShortName
				&& tuples.All(tuple => tuple.Item1.Equals(tuple.Item2));
		}

		public override int GetHashCode ()
		{
			return $"{FullName}-{ShortName}".GetHashCode();
		}
	}
}

