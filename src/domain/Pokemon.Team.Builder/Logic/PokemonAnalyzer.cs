using System;
using System.Collections.Generic;
using Pokemon.Team.Builder.Model;
using System.Linq;

namespace Pokemon.Team.Builder
{
	public static class PokemonAnalyzer
	{
		public static List<RankingPokemonDown> GetMostCommonCountersForTeam(List<DetailedPokemonInformation> team, int count){
			var rankedCounters = new Dictionary<RankingPokemonDown, int> ();

			foreach (var pokemon in team) {
				var counterRanking = RankingCreator.CreateRanking (pokemon.RankingPokemonDown, 11);

				rankedCounters = rankedCounters.MergeDictionaries (new[] { counterRanking });
			}

			var orderedCounters = rankedCounters
				.OrderByDescending (pair => pair.Value)
				.Take (count)
				.Select (pair => pair.Key)
				.ToList();

			return orderedCounters;
		}
	}
}

