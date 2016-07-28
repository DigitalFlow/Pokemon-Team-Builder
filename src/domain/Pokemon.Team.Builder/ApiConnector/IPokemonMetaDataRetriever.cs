using System;
using System.Collections.Generic;

namespace Pokemon.Team.Builder
{
	public interface IPokemonMetaDataRetriever : IDisposable
	{
		List<Pokemon> RetrieveAllPokemon();
	}
}

