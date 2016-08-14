using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading.Tasks;
using System.Linq;

namespace Pokemon.Team.Builder
{
	public class PokemonMetaDataRetriever : IPokemonMetaDataRetriever
    {
        private IHttpClient _client;
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();

		public delegate void DataRetrievedEvent(int count, int progress);
		public event DataRetrievedEvent PokemonDataRetrievedEvent;

        public PokemonMetaDataRetriever(IHttpClient client)
        {
            _client = client;

            _client.TimeOut = new TimeSpan(0, 30, 0);
        }

        public async Task<List<Pokemon>> RetrieveAllPokemon()
        {
			var simplePokemonData = RetrievePokemonSimpleData ();

			return await simplePokemonData.ConfigureAwait(false);
        }

		private void RaiseDataRetrievedEvent(int count, int progress)
		{
			if (PokemonDataRetrievedEvent != null) {
				PokemonDataRetrievedEvent (count, progress);
			}
		}

		public async Task AppendAdvancedData(Pokemon pokemon) {
			try {
				var url = $"api/v2/pokemon-species/{pokemon.Id}";

				var json = await _client.GetStringAsync (url).ConfigureAwait(false);
				var advancedData = JsonConvert.DeserializeObject<AdvancedMetaDataResponse>(json);

				pokemon.Names = advancedData.names;
				pokemon.TextEntries = advancedData.flavor_text_entries;
				pokemon.Varieties = advancedData.varieties;
			}
			catch (Exception ex) {
				Logger.Error($"An error occured while retrieving picture for pokemon #{pokemon.Id}, message {ex.Message}");
			}
		}

		public async Task AppendImage(Pokemon pokemon) {
			try {
				var url = $"media/sprites/pokemon/{pokemon.Id}.png";

                using (var defaultFrontSprite = await _client.GetAsync (url).ConfigureAwait(false))
				{
					var imageBytes = defaultFrontSprite.Content.ReadAsByteArrayAsync ().Result;
					pokemon.Image = Convert.ToBase64String (imageBytes);
				}
			}
			catch (Exception ex) {
				Logger.Error($"An error occured while retrieving picture for pokemon #{pokemon.Id}, message {ex.Message}");
			}
		}

		public async Task<List<Pokemon>> RetrievePokemonSimpleData() {
			var pokemon = new List<Pokemon>();

			var url = "api/v2/pokemon/";
			RetrievePokemonResponse response;

			var progress = 0;

			do
			{
				var json = await _client.GetStringAsync(url).ConfigureAwait(false);

                var requests = new List<Task>();

				response = JsonConvert.DeserializeObject<RetrievePokemonResponse>(json);
				url = response.Next;

                foreach (var item in response.Results)
                {
                    requests.Add(Task.Run(async () =>
                    {
                        RaiseDataRetrievedEvent(response.Count, ++progress);

                        var idMatch = Regex.Match(item.Url, "[0-9]*/$", RegexOptions.Compiled | RegexOptions.CultureInvariant);
                        var id = 0;

                        if (!idMatch.Success)
                        {
                            Logger.Warn($"Failed to find ID in url {item.Url}");
                            return;
                        }

                        var idString = idMatch.Value.Trim(new[] { '/' });

                        if (!int.TryParse(idString, out id))
                        {
                            Logger.Warn($"Failed to parse int {idString}");
                            return;
                        }

                        var poke = new Pokemon
                        {
                            Id = id,
                            Url = item.Url
                        };

                        var tasks = new List<Task>();

                        tasks.Add(AppendImage(poke));
                        tasks.Add(AppendAdvancedData(poke));

                        await Task.WhenAll(tasks.ToArray());

                        pokemon.Add(poke);
                    }));
                }

                await Task.WhenAll(requests.ToArray());

				return pokemon
					.OrderBy(poke => poke.Id)
					.ToList();
			}
			while (!string.IsNullOrEmpty(response.Next));

			return pokemon
				.OrderBy(poke => poke.Id)
				.ToList();
		}

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
