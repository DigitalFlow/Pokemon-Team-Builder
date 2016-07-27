using Newtonsoft.Json;
using Pokemon.Team.Builder.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon.Team.Builder
{
    public class PokemonUsageRetriever : IDisposable
    {
        private IHttpClient _client;

        public PokemonUsageRetriever(IHttpClient client)
        {
            _client = client;
        }

        public RetrievePokemonUsageResponse GetPokemonUsageInformation(int pokemonId)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri("https://3ds.pokemon-gl.com/frontendApi/gbu/getSeasonPokemonDetail"),
                Method = HttpMethod.Post,
                Content = new FormUrlEncodedContent(new[]
               {
                    new KeyValuePair<string, string>("languageId", "2"),
                    new KeyValuePair<string, string>("seasonId", "117"),
                    new KeyValuePair<string, string>("battleType", "1"),
                    new KeyValuePair<string, string>("timezone", "CEST"),
                    new KeyValuePair<string, string>("pokemonId", $"{pokemonId}-0"),
                    new KeyValuePair<string, string>("displayNumberWaza", "10"),
                    new KeyValuePair<string, string>("displayNumberTokusei", "3"),
                    new KeyValuePair<string, string>("displayNumberSeikaku", "10"),
                    new KeyValuePair<string, string>("displayNumberItem", "10"),
                    new KeyValuePair<string, string>("displayNumberLevel", "10"),
                    new KeyValuePair<string, string>("displayNumberPokemonIn", "10"),
                    new KeyValuePair<string, string>("dispayNumberPokemonDown", "10"),
                    new KeyValuePair<string, string>("displayNumberPokemonDownWaza", "10"),
                    new KeyValuePair<string, string>("timestamp", "1469645951951")
                })
            };

            request.Headers.Add("Referer", "http://3ds.pokemon-gl.com/battle/oras/");
            request.Headers.Add("Origin", "http://3ds.pokemon-gl.com");
            request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:33.0) Gecko/20100101 Firefox/33.0");

            var response = _client.SendAsync(request).Result;
            var content = response.Content.ReadAsStringAsync().Result;

            var pokemonUsageResponse = JsonConvert.DeserializeObject<RetrievePokemonUsageResponse>(content);

            return pokemonUsageResponse;
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
