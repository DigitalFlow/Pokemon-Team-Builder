using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pokemon.Team.Builder
{
	public interface IPokemonMetaDataRetriever : IDisposable
	{
		Task<List<Pokemon>> RetrieveAllPokemon();
	}
}

