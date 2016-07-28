using System;
using System.Collections.Generic;
using System.Linq;

namespace Pokemon.Team.Builder
{
	public class PokemonProposer
	{
		private IPokemonUsageRetriever _pokemonUsageRetriever;

		public PokemonProposer(IPokemonUsageRetriever pokemonUsageRetriever)
		{
			_pokemonUsageRetriever = pokemonUsageRetriever;
		}

		public void GetProposedPokemon(List<int> initialTeam) {

			var proposedMembers = new Dictionary<int, int> ();

			foreach (var teamMember in initialTeam) 
			{
				var rankedMates = GetRankedTeamMembersForPokemon (teamMember);

				foreach (var rankedMate in rankedMates.ToList()) {
					if (initialTeam.Contains (rankedMate.Key)) {
						continue;
					}

					var distantRankedMates = GetRankedTeamMembersForPokemon (rankedMate.Key);

					rankedMates = rankedMates.MergeDictionaries (new [] { distantRankedMates });
				}

				proposedMembers = proposedMembers.MergeDictionaries (new[]{ rankedMates });
			}

			var orderedMembers = proposedMembers
				.Where (member => !initialTeam.Contains (member.Key))
				.OrderByDescending (pair => pair.Value).ToList();

			for (var i = 0; i < proposedMembers.Keys.Count && i < 6 - initialTeam.Count; i++) {
				System.Console.WriteLine ($"#{i+1}: {orderedMembers[i].Key} - {orderedMembers[i].Value}");
			}
		}

		public Dictionary<int, int> GetRankedTeamMembersForPokemon(int pokemonId)
		{
			var pokemonInfo = _pokemonUsageRetriever.GetPokemonUsageInformation(pokemonId);

			var rankedMembers = new Dictionary<int, int> ();

			foreach (var pokemon in pokemonInfo.RankingPokemonIn) {
				if (rankedMembers.ContainsKey (pokemon.MonsNo)) {
					// We retrieve ranks from 1-10, so by substracting rank from 11, we get a score of it
					rankedMembers [pokemon.MonsNo] += (11 - pokemon.Ranking);
				} else {
					rankedMembers[pokemon.MonsNo] = (11 - pokemon.Ranking);
				}
			}

			return rankedMembers;
		}
	}
}

