using System;
using Pokemon.Team.Builder.Model;

namespace Pokemon.Team.Builder
{
	public interface IPokemonUsageRetriever : IDisposable
	{
		DetailedPokemonInformation GetPokemonUsageInformation(PokemonIdentifier identifier, int languageId = 2, int seasonId = 117, int battleType = 1);
	}
}

