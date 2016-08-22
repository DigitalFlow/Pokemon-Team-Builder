using System;
using System.Collections.Generic;
using System.Linq;

namespace Pokemon.Team.Builder
{
	public static class RankingCreator
	{
		public static Dictionary<T, int> CreateRanking<T>(IEnumerable<T> rankables) where T : IRankable, IEquatable<T>
		{
			if (rankables == null) {
				return new Dictionary<T, int> ();
			}

			return rankables
				.Where (rank => rank != null)
				.ToDictionary (rank => rank, rank => rankables.Count() - rank.Ranking);
		}
	}
}

