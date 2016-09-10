using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon.Team.Builder.Logic
{
    public class AbilitydexManager
    {
        private IPokemonMetaDataRetriever _pokemonRetriever;

        public AbilitydexManager(IPokemonMetaDataRetriever pokemonMetaDataRetriever)
        {
            _pokemonRetriever = pokemonMetaDataRetriever;
        }

        public async Task<AbilityDex> GetAbilities(string filePath)
        {
            var abilitydex = await GenericSerializer<AbilityDex>.LoadFromFile(filePath).ConfigureAwait(false);

            if (abilitydex == null)
            {
                var abilities = await _pokemonRetriever.RetrieveAllAbilities().ConfigureAwait(false);

                abilitydex = new AbilityDex(abilities);

                await GenericSerializer<AbilityDex>.SaveToFile(abilitydex, filePath).ConfigureAwait(false);
            }

            return abilitydex;
        }
    }
}
