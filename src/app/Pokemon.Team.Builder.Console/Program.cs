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

					GetProposedPokemon (initialTeam, pokemonUsageRetriever);
                }
            }
        }

		private static void GetProposedPokemon(List<int> initialTeam, PokemonUsageRetriever pokemonUsageRetriever) {

			var proposedMembers = new Dictionary<int, int> ();

			foreach(var teamMember in initialTeam) {
				var pokemonInfo = pokemonUsageRetriever.GetPokemonUsageInformation(teamMember);

				foreach (var pokemon in pokemonInfo.rankingPokemonIn) {
					if (proposedMembers.ContainsKey (pokemon.monsno)) {
						proposedMembers [pokemon.monsno] += (11 - pokemon.ranking);
					} else {
						proposedMembers[pokemon.monsno] = (11 - pokemon.ranking);
					}
				}
			}

			var orderedMembers = proposedMembers.OrderByDescending (pair => pair.Value).ToList();

			for (var i = 0; i < proposedMembers.Keys.Count && i < 6 - initialTeam.Count; i++) {
				System.Console.WriteLine ($"#{i+1}: {orderedMembers[i].Key} - {orderedMembers[i].Value}");
			}
		}
    }
}
