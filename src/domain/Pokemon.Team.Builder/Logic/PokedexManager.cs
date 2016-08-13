﻿using System;
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
			var pokedex = PokedexSerializer.LoadPokedexFromFile ("pokedex.xml");

			if (pokedex == null) {
				pokedex = new Pokedex(_pokemonRetriever.RetrieveAllPokemon ());

				PokedexSerializer.SavePokedexToFile (pokedex, "pokedex.xml");
			}

			return pokedex;
		}
	}
}

