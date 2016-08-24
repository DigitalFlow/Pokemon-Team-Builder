﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pokemon.Team.Builder.Interfaces;
using Pokemon.Team.Builder.ApiConnector;
using System.IO;
using Pokemon.Team.Builder.Serialization;
using Pokemon.Team.Builder.Model.Smogon;
using NLog;

namespace Pokemon.Team.Builder.Logic
{
    public class SmogonStatManager : IPokemonUsageRetriever
    {
        private IHttpClient _client;
        private Pokedex _pokedex;
        private Logger _logger = LogManager.GetCurrentClassLogger();

        public SmogonStatManager(Pokedex pokedex, IHttpClient client)
        {
            _client = client;
            _pokedex = pokedex;
        }

        public void Dispose()
        {
            _client.Dispose();
        }

        private int GetWeightingBaseLine(Tier tier)
        {
            switch (tier.ShortName.ToLowerInvariant())
            {
                case "ou":
                    return 1825;
                default:
                    return 1760;
            }
        }

        private void SetMonsNoOnPokemon<T>(IEnumerable<T> pokemon) where T : IPokemonIdentifiable
        {
            foreach (var poke in pokemon)
            {
                try
                {
                    if (poke.Identifier.MonsNo == 0)
                    {
                        var identifier = _pokedex.GetByName(poke.Identifier.Name).Identifier;

                        identifier.Name = poke.Identifier.Name;

                        poke.Identifier = identifier;
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error($"Failed to find pokemon with ID {poke?.Identifier?.MonsNo} and name {poke?.Identifier?.Name} in pokedex");
                }
            }
        }

        private string GetTierName(Tier tier)
        {
            switch (tier.ShortName.ToUpperInvariant())
            {
                case "AG":
                    return "anythinggoes";
                default:
                    return tier.ShortName.ToLowerInvariant();
            }
        }

        public async Task<IPokemonInformation> GetPokemonUsageInformation(PokemonIdentifier identifier, Tier tier = null, int battleType = 1, int seasonId = 117, int rankingPokemonInCount = 10, int rankingPokemonDownCount = 10, int languageId = 2)
        {
            var tierName = GetTierName(tier);

            var tierDescriptor = $"{tierName}-{GetWeightingBaseLine(tier)}".ToLowerInvariant();
            var fileName = $"{tierDescriptor}.xml";

            var tierInformation = SmogonStatSerializer.LoadStatsFromFile(fileName);

            if (tierInformation == null)
            {
                using (var smogonRetriever = new SmogonStatRetriever(_client))
                {
                    tierInformation = await smogonRetriever.RetrieveStats(tierDescriptor);

                    SmogonStatSerializer.SaveStatsToFile(tierInformation, fileName);
                }
            }

            var pokemon = _pokedex.GetByIdentifier(identifier);

            // For some pokemon (i.E. Keldo) there is a trailing description (i.E. "ordinary") in opposite to its special form
            // Remove this part as the Smogon Stats ommit it
            var namePartBlackList = new List<string>
            {
                "-ordinary",
                "-shield"
            };

            var pokemonName = pokemon.GetName();

            foreach (var namePart in namePartBlackList)
            {
                pokemonName = pokemonName.Replace(namePart, string.Empty);
            }

            var information = tierInformation
                .FirstOrDefault(poke =>
                    poke.Name.Equals(pokemonName, StringComparison.InvariantCultureIgnoreCase));

            if (information == null)
            {

            }

            SetMonsNoOnPokemon(new List<SmogonPokemonStats> { information });
            SetMonsNoOnPokemon(information.TeamMates);
            SetMonsNoOnPokemon(information.ChecksAndCounters);

            return information;
        }
    }
}
