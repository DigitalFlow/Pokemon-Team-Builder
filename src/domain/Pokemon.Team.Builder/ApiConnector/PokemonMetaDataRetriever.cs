using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Pokemon.Team.Builder
{
	public class PokemonMetaDataRetriever : IPokemonMetaDataRetriever
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
                url = response.Next;

                foreach (var item in response.Results)
                {
                    var idMatch = Regex.Match(item.Url, "[0-9]*/$", RegexOptions.Compiled | RegexOptions.CultureInvariant);
                    var id = 0;

                    if (!idMatch.Success)
                    {
                        Logger.Warn($"Failed to find ID in url {item.Url}");
                        continue;
                    }

                    var idString = idMatch.Value.Trim(new[] { '/' });
                    
                    if(!int.TryParse(idString, out id))
                    {
                        Logger.Warn($"Failed to parse int {idString}");
                        continue;
                    }

                    var name = char.ToUpperInvariant(item.Name[0]) + item.Name.Substring(1);

                    pokemon.Add(new Pokemon
                    {
                        Id = id,
                        Name = name,
                        Url = item.Url
                    });
                }
            }
            while (!string.IsNullOrEmpty(response.Next));

            return pokemon;
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
