using System;
using System.Collections.Generic;
using System.Linq;
using Pokemon.Team.Builder.Model;
using Pokemon.Team.Builder.Serialization;
using System.Threading.Tasks;
using Pokemon.Team.Builder.Interfaces;

namespace Pokemon.Team.Builder
{
	public class PokemonProposer
	{
		private IPokemonUsageRetriever _pokemonUsageRetriever;
		private const int TEAM_SIZE = 6;
		private TierList _tierList;
		private Tier _activeTier;
        private Pokedex _pokedex;
		private int _battleType;
		private int _season;
		private int _rankingPokemonInCount;
		private int _rankingPokemonDownCount;
        private int _languageId;

		public PokemonProposer(IPokemonUsageRetriever pokemonUsageRetriever, int battleType, int season, int rankingPokemonInCount, int rankingPokemonDownCount,
			int languageId, TierList tierList, Tier activeTier, Pokedex pokedex)
		{
			_pokemonUsageRetriever = pokemonUsageRetriever;
			_tierList = tierList;
			_activeTier = activeTier;
			_battleType = battleType;
			_season = season;
			_rankingPokemonInCount = rankingPokemonInCount;
			_rankingPokemonDownCount = rankingPokemonDownCount;
            _languageId = languageId;
            _pokedex = pokedex;
		}

		private readonly Func<IPokemonIdentifiable, TierList, Tier, bool> IsInActiveTierOrBelow = (proposal, tierList, activeTier) => 
		{
			var proposalTierEntry = tierList.Get (proposal);

			var isInTierOrBelow = proposalTierEntry.IsInTierOrBelow(activeTier);

            return isInTierOrBelow;
		};

		private static bool IsMega(PokemonTierEntry proposalTierEntry)
		{
			if (proposalTierEntry == null) 
			{
				return false;
			}

			if (string.IsNullOrEmpty(proposalTierEntry.forme))
			{
				return false;
			}

			if (proposalTierEntry.forme.Equals ("Mega", StringComparison.InvariantCultureIgnoreCase)) 
			{
				return true;
			}

			return false;
		}

		private readonly Func<IPokemonIdentifiable, TierList, Tier, List<IPokemonInformation>, bool> NoMoreThanOneMega = (proposal, tierList, activeTier, team) => 
		{
			var proposalTierEntry = tierList.Get (proposal);

			if(!IsMega(proposalTierEntry)) 
			{
				return true;
			}

			var otherMega = team.Any(poke => {
				var tierEntry = tierList.Get(poke);

				return IsMega(tierEntry);
			});

			return !otherMega;
		};

		public async Task<List<IPokemonInformation>> GetProposedPokemonByUsage(List<PokemonIdentifier> initialTeam, List<IPokemonInformation> pokemon = null) {

			if (initialTeam == null || initialTeam.Count == 0) {
				throw new ArgumentException ("Initial team must not be empty or null!", "initialTeam");
			}

			if (pokemon == null) {
				pokemon = new List<IPokemonInformation> ();
			}

			if (pokemon.Count >= TEAM_SIZE) {
				return pokemon;
			}

			var proposedMembers = new Dictionary<ITeamMate, int> ();

			// Retrieve Information on each team member
			foreach (var teamMember in initialTeam) 
			{
				var teamMemberInfo = await GetPokemonDetails (teamMember).ConfigureAwait(false);

				if (!pokemon.Contains (teamMemberInfo)) {
					pokemon.Add (teamMemberInfo);
				}

				var rankedMembers = GetRankedTeamMembersForPokemon (teamMemberInfo);
				proposedMembers = proposedMembers.MergeDictionaries(new []{rankedMembers});
			}

			var orderedMembers = proposedMembers
				.Where (proposal => IsInActiveTierOrBelow(proposal.Key, _tierList, _activeTier))
				.Where (proposal => pokemon.All(poke => poke.Identifier.MonsNo != proposal.Key.Identifier.MonsNo))
				.Where (proposal => NoMoreThanOneMega(proposal.Key, _tierList, _activeTier, pokemon))
				.OrderByDescending (pair => pair.Value)
				.ToList();

			var bestMember = orderedMembers.FirstOrDefault ().Key;

			if (bestMember == null) {
				return pokemon;
			}           

			initialTeam.Add (bestMember.Identifier);

			return await GetProposedPokemonByUsage (initialTeam, pokemon).ConfigureAwait(false);
		}

		/// <summary>
		/// Retrieves pokemon information by pokemon MonsNo / Id
		/// </summary>
		/// <returns>The pokemon details.</returns>
		/// <param name="pokemonId">Pokemon ID / MonsNo.</param>
		private async Task<IPokemonInformation> GetPokemonDetails(PokemonIdentifier pokemonId) {
			var information = await _pokemonUsageRetriever.GetPokemonUsageInformation(pokemonId, _activeTier, _battleType, _season, _rankingPokemonInCount, _rankingPokemonDownCount, _languageId)
				.ConfigureAwait(false);

            var counters = information.GetCounters();

            if (counters != null) 
			{
				counters = counters
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
		private Dictionary<ITeamMate, int> GetRankedTeamMembersForPokemon(IPokemonInformation pokemonInfo)
		{
			var rankedMembers = new Dictionary<ITeamMate, int> ();

            var teamMates = pokemonInfo.GetTeamMates();

            if (teamMates == null) {
				return rankedMembers;
			}

			return RankingCreator.CreateRanking (teamMates);
		}
	}
}

