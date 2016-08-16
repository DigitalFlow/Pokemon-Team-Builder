using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon.Team.Builder.ApiConnector
{
    public class SmogonStatRetriever : ISmogonStatRetriever
    {
        private IHttpClient _client;
        private Logger _logger = LogManager.GetCurrentClassLogger();

        public SmogonStatRetriever(IHttpClient client)
        {
            _client = client;
        }

        public void Dispose()
        {
            _client.Dispose();
        }

        private JToken GetChildNode(JToken jObject, string name)
        {
            return jObject
                .Children()
                .SingleOrDefault(p => p.Path.EndsWith(name));
        }

        public async Task RetrieveStats(string tier)
        {
            var year = DateTime.UtcNow.Year;

            // Newest stats are always for the previous month
            var month = DateTime.UtcNow.Month - 1;

            var weightingBaseLine = GetWeightingBaseLine(tier);

            var url = $"{year}-{month.ToString("00")}/chaos/{tier}-{weightingBaseLine}.json";

            var response = await _client.
                GetStringAsync(url)
                .ConfigureAwait(false);

            var json = (JObject) JsonConvert.DeserializeObject(response);

            var dataRoot = json.Children().SingleOrDefault(c => c.Path == "data")?
                .Children()
                .SingleOrDefault(c => c.Path == "data");

            var pokemonNames = new List<string>();

            foreach (JProperty pokemon in dataRoot.Children())
            {
                var name = pokemon.Name;
                pokemonNames.Add(name);

                var data = pokemon.Value;

                var abilities = GetChildNode(data, "Abilities");
                var items = GetChildNode(data, "Items");
                var rawCount = GetChildNode(data, "['Raw count']");
                var spreads = GetChildNode(data, "Spreads");
                var checksAndCounters = GetChildNode(data, "['Checks and Counters']");
                var teamMates = GetChildNode(data, "Teammates");
                var usage = GetChildNode(data, "usage");
                var moves = GetChildNode(data, "Moves");
                var happiness = GetChildNode(data, "Happiness");
                var viabilityCeiling = GetChildNode(data, "['Viability Ceiling']");
            }
        }

        private int GetWeightingBaseLine(string tier)
        {
            switch (tier.ToLowerInvariant())
            {
                case "ou":
                    return 1825;
                default:
                    return 1760;
            }
        }
    }
}
