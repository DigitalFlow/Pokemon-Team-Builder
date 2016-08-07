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

		public List<DetailedPokemonInformation> GetProposedPokemonByUsage(List<PokemonIdentifier> initialTeam, int battleType, List<DetailedPokemonInformation> pokemon = null) {
			if (initialTeam == null || initialTeam.Count == 0) {
				throw new ArgumentException ("Initial team must not be empty or null!", "initialTeam");
			}

			if (pokemon == null) {
				pokemon = new List<DetailedPokemonInformation> ();
			}

			if (pokemon.Count == TEAM_SIZE) {
				return pokemon;
			}

			var proposedMembers = new Dictionary<RankingPokemonIn, int> ();

			// Retrieve Information on each team member
			foreach (var teamMember in initialTeam) 
			{
				var teamMemberInfo = GetPokemonDetails (teamMember, battleType);

				if (!pokemon.Contains (teamMemberInfo)) {
					pokemon.Add (teamMemberInfo);
				}

				var rankedMembers = GetRankedTeamMembersForPokemon (teamMemberInfo);
				proposedMembers = proposedMembers.MergeDictionaries(new []{rankedMembers});
			}

			var orderedMembers = proposedMembers
				.Where (proposal => pokemon.All(poke => (PokemonIdentifier) poke != (PokemonIdentifier) proposal.Key))
				.OrderByDescending (pair => pair.Value)
				.ToList();

			var bestMember = orderedMembers.FirstOrDefault ().Key;

			if (bestMember == null) {
				return pokemon;
			}

			initialTeam.Add (bestMember);
			pokemon.Add (GetPokemonDetails (bestMember, battleType));

			return GetProposedPokemonByUsage (initialTeam, battleType, pokemon);
		}

		/// <summary>
		/// Retrieves pokemon information by pokemon MonsNo / Id
		/// </summary>
		/// <returns>The pokemon details.</returns>
		/// <param name="pokemonId">Pokemon ID / MonsNo.</param>
		private DetailedPokemonInformation GetPokemonDetails(PokemonIdentifier pokemonId, int battleType) {
			return _pokemonUsageRetriever.GetPokemonUsageInformation(pokemonId, battleType);
		}

		/// <summary>
		/// Creates a ranking for the most often used team members. The higher the rank, the better
		/// </summary>
		/// <returns>The ranked team members for pokemon.</returns>
		/// <param name="pokemonInfo">Pokemon info.</param>
		private Dictionary<RankingPokemonIn, int> GetRankedTeamMembersForPokemon(DetailedPokemonInformation pokemonInfo)
		{
			var rankedMembers = new Dictionary<RankingPokemonIn, int> ();

			if (pokemonInfo.RankingPokemonIn == null) {
				return rankedMembers;
			}

			return RankingCreator.CreateRanking (pokemonInfo.RankingPokemonIn, 11);
		}
	}
}

