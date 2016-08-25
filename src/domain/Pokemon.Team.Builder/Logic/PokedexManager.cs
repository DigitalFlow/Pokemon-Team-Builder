using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pokemon.Team.Builder
{
	public class PokedexManager
	{
		private IPokemonMetaDataRetriever _pokemonRetriever;

		public PokedexManager (IPokemonMetaDataRetriever pokemonMetaDataRetriever)
		{
			_pokemonRetriever = pokemonMetaDataRetriever;
		}

		public async Task<Pokedex> GetPokemon(){
			var pokedex = GenericSerializer<Pokedex>.LoadFromFile ("pokedex.xml");

			if (pokedex == null) {
                var pokemon = await _pokemonRetriever.RetrieveAllPokemon().ConfigureAwait(false);

                pokedex = new Pokedex(pokemon);

				GenericSerializer<Pokedex>.SaveToFile (pokedex, "pokedex.xml");
			}

			return pokedex;
		}
	}
}

