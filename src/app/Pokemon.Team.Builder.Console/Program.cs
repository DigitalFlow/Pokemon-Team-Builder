using System;
using System.IO;
using System.Linq;
using NLog;
using System.Collections.Generic;
using Pokemon.Team.Builder.ApiConnector;

namespace Pokemon.Team.Builder.Console
{
    class Program
    {
		private static Logger _logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
			// ProposeTeamMembers (args);
			 using (var httpClient = new HttpClientWrapper (new Uri ("http://www.smogon.com/stats/"))) {
				using (var smogonRetriever = new SmogonStatRetriever (httpClient)) {
                    var stats = smogonRetriever.RetrieveStats ("ou").Result;
				}
			}
        }

        /*private static void ProposeTeamMembers(string[] args){
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
		}*/
	}
}
