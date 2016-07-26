using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Pokemon.Team.Builder
{
    public class PokemonMetaDataRetriever : IDisposable
    {
        private IHttpClient _client;
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public PokemonMetaDataRetriever(IHttpClient client)
        {
            _client = client;
        }

        public List<Pokemon> RetrieveAllPokemon()
        {
            var pokemon = new List<Pokemon>();
            var url = "pokemon";
            RetrievePokemonResponse response;
            
            do
            {
                var json = _client.GetStringAsync(url).Result;
                response = JsonConvert.DeserializeObject<RetrievePokemonResponse>(json);
                url = response.next;

                foreach (var item in response.results)
                {
                    var idMatch = Regex.Match(item.url, "[0-9]*/$", RegexOptions.Compiled | RegexOptions.CultureInvariant);
                    var id = 0;

                    if (!idMatch.Success)
                    {
                        Logger.Warn($"Failed to find ID in url {item.url}");
                        continue;
                    }

                    var idString = idMatch.Value.Trim(new[] { '/' });
                    
                    if(!int.TryParse(idString, out id))
                    {
                        Logger.Warn($"Failed to parse int {idString}");
                        continue;
                    }

                    var name = char.ToUpperInvariant(item.name[0]) + item.name.Substring(1);

                    pokemon.Add(new Pokemon
                    {
                        Id = id,
                        Name = name,
                        Url = item.url
                    });
                }
            }
            while (!string.IsNullOrEmpty(response.next));

            return pokemon;
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
