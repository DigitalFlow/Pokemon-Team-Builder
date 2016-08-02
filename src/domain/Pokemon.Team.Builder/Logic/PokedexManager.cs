using System;
using System.Collections.Generic;

namespace Pokemon.Team.Builder
{
	public class PokedexManager
	{
		private IPokemonMetaDataRetriever _pokemonRetriever;

		public PokedexManager (IPokemonMetaDataRetriever pokemonMetaDataRetriever)
		{
			_pokemonRetriever = pokemonMetaDataRetriever;
		}

		public Pokedex GetPokemon(){
			var pokemon = PokedexSerializer.LoadPokedexFromFile ("pokedex.xml");

			if (pokemon == null) {
				pokemon = _pokemonRetriever.RetrieveAllPokemon ();

				PokedexSerializer.SavePokedexToFile (pokemon, "pokedex.xml");
			}

			return new Pokedex(pokemon);
		}
	}
}

