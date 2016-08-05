using System;
using System.Collections.Generic;
using Pokemon.Team.Builder.Model;
using System.Linq;

namespace Pokemon.Team.Builder
{
	public static class PokemonAnalyzer
	{
		public static List<T> GetRanking<T>(List<DetailedPokemonInformation> team, Func<DetailedPokemonInformation, List<T>> selector, int count, 
			Func<T, bool> filterFunc = null) where T : IRankable, IEquatable<T>
		{ 
			var ranked = new Dictionary<T, int> ();

			foreach (var pokemon in team) {
				var selection = selector (pokemon);

				if (filterFunc != null) {
					selection = selection
						.Where(filterFunc)
						.ToList();
				}

				var counterRanking = RankingCreator.CreateRanking (selection, 11);

				ranked = ranked.MergeDictionaries (new[] { counterRanking });
			}

			var orderedCounters = ranked
				.OrderByDescending (pair => pair.Value)
				.Take (count)
				.Select (pair => pair.Key)
				.ToList();

			return orderedCounters;
		}
	}
}

