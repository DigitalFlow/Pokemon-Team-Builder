using System;
using Pokemon.Team.Builder.Model;
using System.Threading.Tasks;

namespace Pokemon.Team.Builder
{
	public interface IPokemonUsageRetriever : IDisposable
	{
		Task<DetailedPokemonInformation> GetPokemonUsageInformation(PokemonIdentifier identifier, int battleType = 1, int seasonId = 117, int rankingPokemonInCount = 10, 
			int rankingPokemonDownCount = 10, int languageId = 2);
	}
}

