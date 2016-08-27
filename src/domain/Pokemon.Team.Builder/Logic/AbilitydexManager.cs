using System;
using System.Collections.Generic;
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

        public async Task<AbilityDex> GetAbilities()
        {
            var abilitydex = await GenericSerializer<AbilityDex>.LoadFromFile("abilitydex.xml").ConfigureAwait(false);

            if (abilitydex == null)
            {
                var abilities = await _pokemonRetriever.RetrieveAllAbilities().ConfigureAwait(false);

                abilitydex = new AbilityDex(abilities);

                await GenericSerializer<AbilityDex>.SaveToFile(abilitydex, "abilitydex.xml").ConfigureAwait(false);
            }

            return abilitydex;
        }
    }
}
