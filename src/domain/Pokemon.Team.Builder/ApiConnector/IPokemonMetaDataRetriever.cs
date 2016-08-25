using Pokemon.Team.Builder.Logic;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pokemon.Team.Builder
{
	public interface IPokemonMetaDataRetriever : IDisposable
	{
		Task<List<Pokemon>> RetrieveAllPokemon();
        Task<List<Move>> RetrieveAllItems();
        Task<List<Move>> RetrieveAllMoves();
        Task<List<Ability>> RetrieveAllAbilities();
    }
}

