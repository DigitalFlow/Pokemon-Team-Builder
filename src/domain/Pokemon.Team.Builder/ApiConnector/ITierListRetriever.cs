using System;
using System.Collections.Generic;

namespace Pokemon.Team.Builder
{
	public interface ITierListRetriever : IDisposable
	{
		List<PokemonTierEntry> RetrieveTierLists();
	}
}

