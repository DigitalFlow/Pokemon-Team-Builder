using System;
using Pokemon.Team.Builder.Model;
using System.Threading.Tasks;

namespace Pokemon.Team.Builder
{
	public interface IPokemonUsageRetriever : IDisposable
	{
		DetailedPokemonInformation GetPokemonUsageInformation(PokemonIdentifier identifier, int battleType = 1, int languageId = 2, int seasonId = 117);
	}
}

