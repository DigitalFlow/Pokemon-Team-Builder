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
			ProposeTeamMembers (args);

			/*
			 using (var httpClient = new HttpClientWrapper (new Uri ("https://play.pokemonshowdown.com/data/"))) {
				using (var tierRetriever = new TierListRetriever (httpClient)) {

					var tierManager = new TierListManager (tierRetriever);

					tierManager.GetTierList ();
				}
			}
			 */ 
        }

		private static void ProposeTeamMembers(string[] args){
			using (var httpClient = new HttpClientWrapper(new Uri("http://3ds.pokemon-gl.com")))
			{
				using(var pokemonUsageRetriever = new PokemonUsageRetriever(httpClient))
				{
					var initialTeam = args.Select(arg => new PokemonIdentifier(Int32.Parse(arg))).ToList();

					var pokemonProposer = new PokemonProposer (pokemonUsageRetriever, 1, 117, 10, 10, 2, new TierList(new List<PokemonTierEntry>()), new Tier());

					var proposedTeam = pokemonProposer.GetProposedPokemonByUsage (initialTeam).Result;

					for (var i = 0; i < proposedTeam.Count; i++) {
						_logger.Info ($"Pokemon Team Member #{i+1}: {proposedTeam[i].RankingPokemonInfo.MonsNo} - {proposedTeam[i].RankingPokemonInfo.Name}");
					}
				}
			}
		}
	}
}
