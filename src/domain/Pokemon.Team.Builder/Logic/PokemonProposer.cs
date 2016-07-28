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

		public List<int> GetProposedPokemon(List<int> initialTeam) {

			if (initialTeam.Count == 6) {
				return initialTeam;
			}

			var proposedMembers = new Dictionary<int, int> ();

			foreach (var teamMember in initialTeam) 
			{
				var rankedMembers = GetRankedTeamMembersForPokemon (teamMember);

				proposedMembers = proposedMembers.MergeDictionaries(new []{rankedMembers});
			}

			var orderedMembers = proposedMembers
				.Where (member => !initialTeam.Contains (member.Key))
				.OrderByDescending (pair => pair.Value).ToList();

			initialTeam.Add (orderedMembers.First ().Key);

			return GetProposedPokemon (initialTeam);
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

