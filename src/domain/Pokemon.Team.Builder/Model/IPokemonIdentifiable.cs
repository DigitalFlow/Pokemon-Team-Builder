using System;

namespace Pokemon.Team.Builder
{
	public interface IPokemonIdentifiable
	{
		PokemonIdentifier Identifier { get; set; }
	}
}

