using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using Pokemon.Team.Builder.Model.Smogon;
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

                var abilities = ParseAbilities(data);
                var items = ParseItems(data);
                var rawCount = ParseRawCount(data);
                var spreads = ParseSpreads(data);
                var checksAndCounters = ParseChecksAndCounters(data);
                var teamMates = ParseTeamMates(data);
                var usage = ParseUsage(data);
                var moves = ParseMoves(data);
                var happiness = ParseHappiness(data);
                var viabilityCeiling = ParseViabilityCeiling(data);
            }
        }

        private List<int> ParseViabilityCeiling(JToken data)
        {
            var node = GetChildNode(data, "['Viability Ceiling']");

            return node
                .Children()
                .SingleOrDefault()
                .Value<Array>()
                .OfType<int>()
                .ToList();
        }

        private List<SmogonHappiness> ParseHappiness(JToken data)
        {
            var happinessRoot = GetChildNode(data, "Happiness");

            var happinessNodes = happinessRoot.Children().SingleOrDefault();

            var happiness = happinessNodes.Children()
                .Select(node => new SmogonHappiness { Name = ((JProperty)node).Name, UsageRate = node.Children().SingleOrDefault().Value<float>() })
                .ToList();

            return happiness;
        }

        private float ParseUsage(JToken data)
        {
            return GetChildNode(data, "usage").Children().SingleOrDefault().Value<float>();
        }

        private int ParseRawCount(JToken data)
        {
            return GetChildNode(data, "['Raw count']")
                .Children()
                .SingleOrDefault()
                .Value<int>();
        }

        private List<SmogonTeamMate> ParseTeamMates(JToken data)
        {
            var teamRoot = GetChildNode(data, "Teammates");

            var teamNodes = teamRoot.Children().SingleOrDefault();

            var team = teamNodes.Children()
                .Select(node => new SmogonTeamMate { Name = ((JProperty)node).Name, UsageRate = node.Children().SingleOrDefault().Value<float>() })
                .ToList();

            return team;
        }

        private List<SmogonCheck> ParseChecksAndCounters(JToken data)
        {
            var checksRoot = GetChildNode(data, "['Checks and Counters']");

            var checksNodes = checksRoot.Children().SingleOrDefault();

            var checks = checksNodes.Children()
                .Select(node => new SmogonCheck { Name = ((JProperty)node).Name, UsageRate = node.Children().SingleOrDefault().Value<float>() })
                .ToList();

            return checks;
        }

        private List<SmogonSpread> ParseSpreads(JToken data)
        {
            var spreadRoot = GetChildNode(data, "Spreads");

            var spreadNodes = spreadRoot.Children().SingleOrDefault();

            var spreads = spreadNodes.Children()
                .Select(node => new SmogonSpread { Name = ((JProperty)node).Name, UsageRate = node.Children().SingleOrDefault().Value<float>() })
                .ToList();

            return spreads;
        }

        private List<SmogonMove> ParseMoves(JToken data)
        {
            var moveRoot = GetChildNode(data, "Moves");

            var moveNodes = moveRoot.Children().SingleOrDefault();

            var moves = moveNodes.Children()
                .Select(node => new SmogonMove { Name = ((JProperty)node).Name, UsageRate = node.Children().SingleOrDefault().Value<float>() })
                .ToList();

            return moves;
        }

        private List<SmogonItem> ParseItems(JToken data)
        {
            var itemRoot = GetChildNode(data, "Items");

            var itemNodes = itemRoot.Children().SingleOrDefault();

            var items = itemNodes.Children()
                .Select(node => new SmogonItem { Name = ((JProperty)node).Name, UsageRate = node.Children().SingleOrDefault().Value<float>() })
                .ToList();

            return items;
        }

        private List<SmogonAbility> ParseAbilities(JToken data)
        {
            var abilitiesRoot = GetChildNode(data, "Abilities");

            var abilityNodes = abilitiesRoot.Children().SingleOrDefault();

            var abilities = abilityNodes.Children()
                .Select(node => new SmogonAbility { Name = ((JProperty)node).Name, UsageRate = node.Children().SingleOrDefault().Value<float>() })
                .ToList();

            return abilities;
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
