﻿using System;
using System.IO;

namespace Pokemon.Team.Builder.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var httpClient = new HttpClientWrapper(new Uri("http://3ds.pokemon-gl.com")))
            {
                //using (var httpClient = new HttpClientWrapper(new Uri("http://Pokemonapi.co/api/v2/")))
                //{
                //using (var pokemonRetriever = new PokemonMetaDataRetriever(httpClient))
                //{
                //    var retriever = new PokemonMetaDataRetriever(httpClient);

                //    var pokemon = retriever.RetrieveAllPokemon();

                //    File.WriteAllText("pokedex.xml", PokedexSerializer.SerializePokedex(pokemon));
                //}

                using(var pokemonUsageRetriever = new PokemonUsageRetriever(httpClient))
                {
                    var pokemonInfo = pokemonUsageRetriever.GetPokemonUsageInformation(445);
                }
            }
        }
    }
}
