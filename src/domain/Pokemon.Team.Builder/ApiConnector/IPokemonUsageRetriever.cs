using System;
using Pokemon.Team.Builder.Model;

namespace Pokemon.Team.Builder
{
	public interface IPokemonUsageRetriever : IDisposable
	{
		RetrievePokemonUsageResponse GetPokemonUsageInformation(int pokemonId, int formNo = 0, int languageId = 2, int seasonId = 117, int battleType = 1);
	}
}

