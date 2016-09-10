using System;
using System.Collections.Generic;
using System.IO;
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

		public async Task<Pokedex> GetPokemon(string filePath){
            var pokedex = await GenericSerializer<Pokedex>.LoadFromFile (filePath).ConfigureAwait(false);

			if (pokedex == null) {
                var pokemon = await _pokemonRetriever.RetrieveAllPokemon().ConfigureAwait(false);

                pokedex = new Pokedex(pokemon);

				await GenericSerializer<Pokedex>.SaveToFile (pokedex, filePath).ConfigureAwait(false);
			}

			return pokedex;
		}
	}
}

