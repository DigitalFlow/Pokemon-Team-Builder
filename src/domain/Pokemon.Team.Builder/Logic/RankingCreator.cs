using System;
using System.Collections.Generic;
using System.Linq;

namespace Pokemon.Team.Builder
{
	public static class RankingCreator
	{
		public static Dictionary<T, int> CreateRanking<T>(List<T> rankables, int rankingSubtractor) where T : IRankable
		{
			return rankables
				.ToDictionary (rank => rank, rank => rankingSubtractor - rank.Ranking);
		}
	}
}

