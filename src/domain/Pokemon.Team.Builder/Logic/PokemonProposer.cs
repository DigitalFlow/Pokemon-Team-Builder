using System;
using System.Collections.Generic;
using System.Linq;
using Pokemon.Team.Builder.Model;

namespace Pokemon.Team.Builder
{
	public class PokemonProposer
	{
		private IPokemonUsageRetriever _pokemonUsageRetriever;
		private const int TEAM_SIZE = 6;

		public PokemonProposer(IPokemonUsageRetriever pokemonUsageRetriever)
		{
			_pokemonUsageRetriever = pokemonUsageRetriever;
		}

		public List<Pokemon> GetProposedPokemon(List<int> initialTeam, List<Pokemon> pokemon = null) {
			// Run one additional time so that last member is also added to pokemon list
			if (initialTeam.Count == TEAM_SIZE + 1) {
				return pokemon;
			}

			if (pokemon == null) {
				pokemon = new List<Pokemon> ();
			}

			var proposedMembers = new Dictionary<int, int> ();

			foreach (var teamMember in initialTeam) 
			{
				var rankedMembers = GetRankedTeamMembersForPokemon (teamMember);

				if (pokemon.All (poke => poke.Id != teamMember)) {
					pokemon.Add (new Pokemon {
						Id = teamMember,
						Name = rankedMembers.Item1.RankingPokemonInfo.Name
					});
				}

				proposedMembers = proposedMembers.MergeDictionaries(new []{rankedMembers.Item2.ToDictionary(member => member.Key.MonsNo, member => member.Value)});
			}

			var orderedMembers = proposedMembers
				.Where (member => !initialTeam.Contains (member.Key))
				.OrderByDescending (pair => pair.Value).ToList();

			initialTeam.Add (orderedMembers.First ().Key);

			return GetProposedPokemon (initialTeam, pokemon);
		}

		public Tuple<RetrievePokemonUsageResponse, Dictionary<RankingPokemonIn, int>> GetRankedTeamMembersForPokemon(int pokemonId)
		{
			var pokemonInfo = _pokemonUsageRetriever.GetPokemonUsageInformation(pokemonId);

			var rankedMembers = new Dictionary<RankingPokemonIn, int> ();

			foreach (var pokemon in pokemonInfo.RankingPokemonIn) {
				if (rankedMembers.ContainsKey (pokemon)) {
					// We retrieve ranks from 1-10, so by substracting rank from 11, we get a score of it
					rankedMembers [pokemon] += (11 - pokemon.Ranking);
				} else {
					rankedMembers[pokemon] = (11 - pokemon.Ranking);
				}
			}

			return new Tuple<RetrievePokemonUsageResponse, Dictionary<RankingPokemonIn, int>>(pokemonInfo, rankedMembers);
		}
	}
}

