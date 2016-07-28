using System;
using Pokemon.Team.Builder.Model;

namespace Pokemon.Team.Builder
{
	public interface IPokemonUsageRetriever : IDisposable
	{
		RetrievePokemonUsageResponse GetPokemonUsageInformation(int pokemonId);
	}
}

