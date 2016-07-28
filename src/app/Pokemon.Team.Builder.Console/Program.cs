using System;
using System.IO;
using System.Linq;
using NLog;
using System.Collections.Generic;

namespace Pokemon.Team.Builder.Console
{
    class Program
    {
		private static Logger _logger = LogManager.GetCurrentClassLogger();

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
					var initialTeam = args.Select(arg => Int32.Parse(arg)).ToList();

					var pokemonProposer = new PokemonProposer (pokemonUsageRetriever);

					var proposedTeam = pokemonProposer.GetProposedPokemon (initialTeam);

					for (var i = 0; i < proposedTeam.Count; i++) {
						_logger.Info ($"Pokemon Team Member #{i+1}: {proposedTeam[i]}");
					}
                }
            }
        }
    }
}
