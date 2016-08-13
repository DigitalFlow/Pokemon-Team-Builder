using System;
using System.Collections.Generic;
using System.Linq;
using Pokemon.Team.Builder.Model;
using Pokemon.Team.Builder.Serialization;

namespace Pokemon.Team.Builder
{
	public class PokemonProposer
	{
		private IPokemonUsageRetriever _pokemonUsageRetriever;
		private const int TEAM_SIZE = 6;
		private TierList _tierList;
		private Tier _activeTier;
		private int _battleType;
		private int _season;
		private int _rankingPokemonInCount;
		private int _rankingPokemonDownCount;
        private int _languageId;

		public PokemonProposer(IPokemonUsageRetriever pokemonUsageRetriever, int battleType, int season, int rankingPokemonInCount, int rankingPokemonDownCount,
			int languageId, TierList tierList, Tier activeTier)
		{
			_pokemonUsageRetriever = pokemonUsageRetriever;
			_tierList = tierList;
			_activeTier = activeTier;
			_battleType = battleType;
			_season = season;
			_rankingPokemonInCount = rankingPokemonInCount;
			_rankingPokemonDownCount = rankingPokemonDownCount;
            _languageId = languageId;
		}

		private readonly Func<IPokemonIdentifiable, TierList, Tier, bool> IsInActiveTierOrBelow = (proposal, tierList, activeTier) => 
		{
			var proposalTierEntry = tierList.GetById (proposal.MonsNo, proposal.FormNo);

			return proposalTierEntry.IsInTierOrBelow(activeTier);
		};

		public List<DetailedPokemonInformation> GetProposedPokemonByUsage(List<PokemonIdentifier> initialTeam, List<DetailedPokemonInformation> pokemon = null) {

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
				var teamMemberInfo = GetPokemonDetails (teamMember);

				if (!pokemon.Contains (teamMemberInfo)) {
					pokemon.Add (teamMemberInfo);
				}

				var rankedMembers = GetRankedTeamMembersForPokemon (teamMemberInfo);
				proposedMembers = proposedMembers.MergeDictionaries(new []{rankedMembers});
			}

			var orderedMembers = proposedMembers
				.Where (proposal => IsInActiveTierOrBelow(proposal.Key, _tierList, _activeTier))
				.Where (proposal => pokemon.All(poke => (PokemonIdentifier) poke != (PokemonIdentifier) proposal.Key))
				.OrderByDescending (pair => pair.Value)
				.ToList();

			var bestMember = orderedMembers.FirstOrDefault ().Key;

			if (bestMember == null) {
				return pokemon;
			}

			initialTeam.Add (bestMember);
			pokemon.Add (GetPokemonDetails (bestMember));

			return GetProposedPokemonByUsage (initialTeam, pokemon);
		}

		/// <summary>
		/// Retrieves pokemon information by pokemon MonsNo / Id
		/// </summary>
		/// <returns>The pokemon details.</returns>
		/// <param name="pokemonId">Pokemon ID / MonsNo.</param>
		private DetailedPokemonInformation GetPokemonDetails(PokemonIdentifier pokemonId) {
			var information = _pokemonUsageRetriever.GetPokemonUsageInformation(pokemonId, _battleType, _season, _rankingPokemonInCount, _rankingPokemonDownCount, _languageId);

			if (information.RankingPokemonDown != null) 
			{
				information.RankingPokemonDown = information.RankingPokemonDown
					.Where (poke => IsInActiveTierOrBelow (poke, _tierList, _activeTier))
					.ToList ();
			}
            
			return information;
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

			return RankingCreator.CreateRanking (pokemonInfo.RankingPokemonIn);
		}
	}
}

