using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using Pokemon.Team.Builder.Model.Smogon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pokemon.Team.Builder.ApiConnector
{
    public class SmogonStatRetriever : ISmogonStatRetriever
    {
        private IHttpClient _client;
        private Logger _logger = LogManager.GetCurrentClassLogger();
        private const double UsageRateMinimum = 1;

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

        public static string GetFileName(string tierDescriptor)
        {
            var year = DateTime.UtcNow.Year;

            // Newest stats are always for the previous month
            var month = DateTime.UtcNow.Month - 1;

            var url = $"{year}-{month.ToString("00")}/chaos/{tierDescriptor}.json";

            return url;
        }

        public async Task<List<SmogonPokemonStats>> RetrieveStats(string tierDescriptor)
        {
            var url = GetFileName(tierDescriptor);

            var response = await _client.
                GetStringAsync(url)
                .ConfigureAwait(false);

            var json = (JObject)JsonConvert.DeserializeObject(response);

            var dataRoot = json.Children().SingleOrDefault(c => c.Path == "data")?
                .Children()
                .SingleOrDefault(c => c.Path == "data");

            var pokemon = new List<SmogonPokemonStats>();

            foreach (JProperty entry in dataRoot.Children())
            {
                var pokemonStat = new SmogonPokemonStats();

                pokemonStat.Name = entry.Name;

                var data = entry.Value;

                pokemonStat.Abilities = ParseAbilities(data);
                pokemonStat.Items = ParseItems(data);
                pokemonStat.RawCount = ParseRawCount(data);
                pokemonStat.Spreads = ParseSpreads(data);
                pokemonStat.ChecksAndCounters = ParseChecksAndCounters(data);
                pokemonStat.TeamMates = ParseTeamMates(data);
                pokemonStat.Usage = ParseUsage(data);
                pokemonStat.Moves = ParseMoves(data);
                pokemonStat.Happiness = ParseHappiness(data);
                pokemonStat.ViabilityCeiling = ParseViabilityCeiling(data);

                pokemon.Add(pokemonStat);
            }

            return pokemon;
        }

        private List<int> ParseViabilityCeiling(JToken data)
        {
            var node = GetChildNode(data, "['Viability Ceiling']");

            return node
                .Children()
                .SingleOrDefault()
                .Values<int>()
                .ToList();
        }

        private List<SmogonHappiness> ParseHappiness(JToken data)
        {
            var happinessRoot = GetChildNode(data, "Happiness");

            var happinessNodes = happinessRoot.Children().SingleOrDefault();

            var happiness = happinessNodes.Children()
                .Select(node => new SmogonHappiness { Name = ((JProperty)node).Name, UsageRate = node.Children().SingleOrDefault().Value<float>() })
                .ToList();

            happiness = happiness.Where(happy => happy.UsageRate > UsageRateMinimum).ToList();

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

            team = team.Where(t => t.UsageRate > UsageRateMinimum).ToList();

            return team;
        }

        private List<SmogonCheck> ParseChecksAndCounters(JToken data)
        {
            var checksRoot = GetChildNode(data, "['Checks and Counters']");

            var checksNodes = checksRoot.Children().SingleOrDefault();

            var checks = checksNodes.Children()
            .Select(node => new SmogonCheck
            {
                Name = ((JProperty)node).Name,
                Statistics = node.Children().SingleOrDefault().Values<float>().ToList()
            })
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

            spreads = spreads.Where(spread => spread.UsageRate > UsageRateMinimum).ToList();

            return spreads;
        }

        private List<SmogonMove> ParseMoves(JToken data)
        {
            var moveRoot = GetChildNode(data, "Moves");

            var moveNodes = moveRoot.Children().SingleOrDefault();

            var moves = moveNodes.Children()
                .Select(node => new SmogonMove { Name = ((JProperty)node).Name, UsageRate = node.Children().SingleOrDefault().Value<float>() })
                .ToList();

            moves = moves.Where(move => move.UsageRate > UsageRateMinimum).ToList();

            return moves;
        }

        private List<SmogonItem> ParseItems(JToken data)
        {
            var itemRoot = GetChildNode(data, "Items");

            var itemNodes = itemRoot.Children().SingleOrDefault();

            var items = itemNodes.Children()
                .Select(node => new SmogonItem { Name = ((JProperty)node).Name, UsageRate = node.Children().SingleOrDefault().Value<float>() })
                .ToList();

            items = items.Where(item => item.UsageRate > UsageRateMinimum).ToList();

            return items;
        }

        private List<SmogonAbility> ParseAbilities(JToken data)
        {
            var abilitiesRoot = GetChildNode(data, "Abilities");

            var abilityNodes = abilitiesRoot.Children().SingleOrDefault();

            var abilities = abilityNodes.Children()
                .Select(node => new SmogonAbility { Name = ((JProperty)node).Name, UsageRate = node.Children().SingleOrDefault().Value<float>() })
                .ToList();

            abilities = abilities.Where(ability => ability.UsageRate > UsageRateMinimum).ToList();

            return abilities;
        }
    }
}
