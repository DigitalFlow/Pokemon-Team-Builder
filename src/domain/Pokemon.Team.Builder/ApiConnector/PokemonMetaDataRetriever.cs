using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading.Tasks;

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
        }

        public List<Pokemon> RetrieveAllPokemon()
        {
			var simplePokemonData = RetrievePokemonSimpleData ();

			return simplePokemonData;
        }

		private void RaiseDataRetrievedEvent(int count, int progress)
		{
			if (PokemonDataRetrievedEvent != null) {
				PokemonDataRetrievedEvent (count, progress);
			}
		}

		public void AppendImage(Pokemon pokemon) {
			try {
				var url = $"media/sprites/pokemon/{pokemon.Id}.png";

                using (var defaultFrontSprite = _client.GetAsync (url).Result)
				{
					var imageBytes = defaultFrontSprite.Content.ReadAsByteArrayAsync ().Result;
					pokemon.Image = Convert.ToBase64String (imageBytes);
				}
			}
			catch (Exception ex) {
				Logger.Error($"An error occured while retrieving picture for pokemon #{pokemon.Id}, message {ex.Message}");
			}
		}

		public List<Pokemon> RetrievePokemonSimpleData() {
			var pokemon = new List<Pokemon>();

			var url = "api/v2/pokemon/";
			RetrievePokemonResponse response;

			var progress = 0;

			do
			{
				var json = _client.GetStringAsync(url).Result;

				response = JsonConvert.DeserializeObject<RetrievePokemonResponse>(json);
				url = response.Next;

				foreach (var item in response.Results) {
					RaiseDataRetrievedEvent(response.Count, ++progress);

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

					var poke = new Pokemon
						{
							Id = id,
							Name = name,
							Url = item.Url
						};

                    AppendImage(poke);
					
					pokemon.Add(poke);
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
