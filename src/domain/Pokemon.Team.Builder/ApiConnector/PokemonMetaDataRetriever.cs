using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using Pokemon.Team.Builder.Logic;
using Pokemon.Team.Builder.Model.PokeAPI;

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

        public async Task<List<Move>> RetrieveAllItems()
        {
            var itemData = RetrieveItemData ();

            return await itemData.ConfigureAwait(false);
        }

        public async Task<List<Move>> RetrieveAllMoves()
        {
            var moveData = RetrieveMoveData();

            return await moveData.ConfigureAwait(false);
        }

        public async Task<List<Ability>> RetrieveAllAbilities()
        {
            var abilityData = RetrieveAbilityData();

            return await abilityData.ConfigureAwait(false);
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

        public async Task<Item> AppendAdvancedData(Item item)
        {
            try
            {
                var url = $"api/v2/item/{item.Id}";

                var json = await _client.GetStringAsync(url).ConfigureAwait(false);
                var advancedData = JsonConvert.DeserializeObject<Item>(json);

                return advancedData;
            }
            catch (Exception ex)
            {
                Logger.Error($"An error occured while retrieving picture for item #{item.Id}, message {ex.Message}");
                return null;
            }
        }

        public async Task<Ability> AppendAdvancedData(Ability ability)
        {
            try
            {
                var url = $"api/v2/ability/{ability.Id}";

                var json = await _client.GetStringAsync(url).ConfigureAwait(false);
                var advancedData = JsonConvert.DeserializeObject<Ability>(json);

                return advancedData;
            }
            catch (Exception ex)
            {
                Logger.Error($"An error occured while retrieving picture for ability #{ability.Id}, message {ex.Message}");
                return null;
            }
        }

        public async Task<Move> AppendAdvancedData(Move move)
        {
            try
            {
                var url = $"api/v2/move/{move.Id}";

                var json = await _client.GetStringAsync(url).ConfigureAwait(false);
                var advancedData = JsonConvert.DeserializeObject<Move>(json);

                return advancedData;
            }
            catch (Exception ex)
            {
                Logger.Error($"An error occured while retrieving picture for move #{move.Id}, message {ex.Message}");
                return null;
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

        public async Task<List<Move>> RetrieveItemData()
        {
            var items = new List<Move>();

            var url = "api/v2/item/";
            ItemOverviewResponse response;

            var progress = 0;

            do
            {
                var json = await _client.GetStringAsync(url).ConfigureAwait(false);

                var requests = new List<Task>();

                response = JsonConvert.DeserializeObject<ItemOverviewResponse>(json);
                url = response.Next;

                foreach (var result in response.Results)
                {
                    requests.Add(Task.Run((Func<Task>)(async () =>
                    {
                        RaiseDataRetrievedEvent(response.Count, ++progress);

                        var idMatch = Regex.Match(result.Url, "[0-9]*/$", RegexOptions.Compiled | RegexOptions.CultureInvariant);
                        var id = 0;

                        if (!idMatch.Success)
                        {
                            Logger.Warn($"Failed to find ID in url {result.Url}");
                            return;
                        }

                        var idString = idMatch.Value.Trim(new[] { '/' });

                        if (!int.TryParse(idString, out id))
                        {
                            Logger.Warn($"Failed to parse int {idString}");
                            return;
                        }

                        var item = new Move
                        {
                            Id = id
                        };

                        items.Add(await AppendAdvancedData(item).ConfigureAwait(false));
                    })));
                }

                await Task.WhenAll(requests.ToArray());
            }
            while (!string.IsNullOrEmpty(response.Next));

            return items
                .OrderBy(poke => poke.Id)
                .ToList();
        }

        public async Task<List<Move>> RetrieveMoveData()
        {
            var moves = new List<Move>();

            var url = "api/v2/move/";
            MoveOverviewResponse response;

            var progress = 0;

            do
            {
                var json = await _client.GetStringAsync(url).ConfigureAwait(false);

                var requests = new List<Task>();

                response = JsonConvert.DeserializeObject<MoveOverviewResponse>(json);
                url = response.Next;

                foreach (var result in response.Results)
                {
                    requests.Add(Task.Run((Func<Task>)(async () =>
                    {
                        RaiseDataRetrievedEvent(response.Count, ++progress);

                        var idMatch = Regex.Match(result.Url, "[0-9]*/$", RegexOptions.Compiled | RegexOptions.CultureInvariant);
                        var id = 0;

                        if (!idMatch.Success)
                        {
                            Logger.Warn($"Failed to find ID in url {result.Url}");
                            return;
                        }

                        var idString = idMatch.Value.Trim(new[] { '/' });

                        if (!int.TryParse(idString, out id))
                        {
                            Logger.Warn($"Failed to parse int {idString}");
                            return;
                        }

                        var move = new Move
                        {
                            Id = id
                        };

                        moves.Add(await AppendAdvancedData(move).ConfigureAwait(false));
                    })));
                }

                await Task.WhenAll(requests.ToArray());
            }
            while (!string.IsNullOrEmpty(response.Next));

            return moves
                .OrderBy(move => move?.Id)
                .ToList();
        }

        public async Task<List<Ability>> RetrieveAbilityData()
        {
            var abilities = new List<Ability>();

            var url = "api/v2/ability/";
            AbilityOverviewResponse response;

            var progress = 0;

            do
            {
                var json = await _client.GetStringAsync(url).ConfigureAwait(false);

                var requests = new List<Task>();

                response = JsonConvert.DeserializeObject<AbilityOverviewResponse>(json);
                url = response.Next;

                foreach (var result in response.Results)
                {
                    requests.Add(Task.Run(async () =>
                    {
                        RaiseDataRetrievedEvent(response.Count, ++progress);

                        var idMatch = Regex.Match(result.Url, "[0-9]*/$", RegexOptions.Compiled | RegexOptions.CultureInvariant);
                        var id = 0;

                        if (!idMatch.Success)
                        {
                            Logger.Warn($"Failed to find ID in url {result.Url}");
                            return;
                        }

                        var idString = idMatch.Value.Trim(new[] { '/' });

                        if (!int.TryParse(idString, out id))
                        {
                            Logger.Warn($"Failed to parse int {idString}");
                            return;
                        }

                        var item = new Ability
                        {
                            Id = id
                        };

                        abilities.Add(await AppendAdvancedData(item).ConfigureAwait(false));
                    }));
                }

                await Task.WhenAll(requests.ToArray());
            }
            while (!string.IsNullOrEmpty(response.Next));

            return abilities
                .OrderBy(poke => poke.Id)
                .ToList();
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
