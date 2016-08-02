﻿using Newtonsoft.Json;
using Pokemon.Team.Builder.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace Pokemon.Team.Builder
{
	public class PokemonUsageRetriever : IPokemonUsageRetriever, IDisposable
    {
        private IHttpClient _client;
		private Logger _logger = LogManager.GetCurrentClassLogger();

        public PokemonUsageRetriever(IHttpClient client)
        {
            _client = client;
        }

		public DetailedPokemonInformation GetPokemonUsageInformation(PokemonIdentifier pokemonId, int languageId = 2, int seasonId = 117, int battleType = 1)
        {
			var unixTimeStamp = (Int32)(DateTime.UtcNow.Subtract (new DateTime (1970, 1, 1))).TotalSeconds;

            var request = new HttpRequestMessage
            {
                RequestUri = new Uri("https://3ds.pokemon-gl.com/frontendApi/gbu/getSeasonPokemonDetail"),
                Method = HttpMethod.Post,
                Content = new FormUrlEncodedContent(new[]
               	{
						new KeyValuePair<string, string>("languageId", $"{languageId}"),
						new KeyValuePair<string, string>("seasonId", $"{seasonId}"),
						new KeyValuePair<string, string>("battleType", $"{battleType}"),
	                    new KeyValuePair<string, string>("timezone", "CEST"),
						new KeyValuePair<string, string>("pokemonId", pokemonId.ToString()),
	                    new KeyValuePair<string, string>("displayNumberWaza", "10"),
	                    new KeyValuePair<string, string>("displayNumberTokusei", "3"),
	                    new KeyValuePair<string, string>("displayNumberSeikaku", "10"),
	                    new KeyValuePair<string, string>("displayNumberItem", "10"),
	                    new KeyValuePair<string, string>("displayNumberLevel", "10"),
	                    new KeyValuePair<string, string>("displayNumberPokemonIn", "10"),
	                    new KeyValuePair<string, string>("dispayNumberPokemonDown", "10"),
	                    new KeyValuePair<string, string>("displayNumberPokemonDownWaza", "10"),
						new KeyValuePair<string, string>("timestamp", "{unixTimeStamp}")
                })
            };

            request.Headers.Add("Referer", "http://3ds.pokemon-gl.com/battle/oras/");
            request.Headers.Add("Origin", "http://3ds.pokemon-gl.com");
            request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:33.0) Gecko/20100101 Firefox/33.0");

			using(var response = _client.SendAsync(request).Result)
			{
				try
				{
            		var content = response.Content.ReadAsStringAsync().Result;

		            var pokemonUsageResponse = JsonConvert.DeserializeObject<DetailedPokemonInformation>(content);

					return pokemonUsageResponse;
				}
				catch (Exception ex) {
					_logger.Error($"Exception during contacting of API, message is {ex.Message}");
					return null;
				}
			}	
    	}

	    public void Dispose()
	    {
	        _client.Dispose();
	    }
	}
}
