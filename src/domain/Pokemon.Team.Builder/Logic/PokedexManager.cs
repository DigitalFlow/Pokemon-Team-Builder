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
			var pokedex = PokedexSerializer.LoadPokedexFromFile ("pokedex.xml");

			if (pokedex == null) {
                var pokemon = await _pokemonRetriever.RetrieveAllPokemon().ConfigureAwait(false);

                pokedex = new Pokedex(pokemon);

				PokedexSerializer.SavePokedexToFile (pokedex, "pokedex.xml");
			}

			return pokedex;
		}
	}
}

