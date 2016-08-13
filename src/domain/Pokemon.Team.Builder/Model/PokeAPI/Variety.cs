using System;

namespace Pokemon.Team.Builder
{
    [Serializable]
	public class Variety
	{
		public bool is_default { get; set; }
		public PokemonDescriptor pokemon { get; set; }
	}
}

