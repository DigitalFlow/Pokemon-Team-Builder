using System;
using System.Linq;
using System.Text.RegularExpressions;
using NLog;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Pokemon.Team.Builder
{
	public class TierListRetriever : ITierListRetriever
	{
		private IHttpClient _client;
		private Logger _logger = LogManager.GetCurrentClassLogger();

		public TierListRetriever(IHttpClient client)
		{
			_client = client;
		}

		public List<PokemonTierEntry> RetrieveTierLists(){
			var js = _client.GetStringAsync ("pokedex.js?v0.9xy31").Result;

			// Remove beginning
			js = js.Replace ("exports.BattlePokedex = {", "");

			// Remove pokemon object names
			js = Regex.Replace (js, "[a-zA-Z0-9]+:{num", "{num");

			js = Regex.Replace (js, "([a-zA-Z0-9]+):", "\"$1\":");

			js = js.Replace ("\"0\":", "\"_0\":");
			js = js.Replace ("\"1\":", "\"_1\":");

			js = js.Substring (0, js.Length - 2);

			js = $"[{js}]";

			var count = Regex.Matches (js, "\"num\":").Count;

			var parsed = JsonConvert.DeserializeObject<List<PokemonTierEntry>> (js);

			return parsed;
		}

		public void Dispose()
		{
			_client.Dispose();
		}
	}
}

