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
            var abilitydex = GenericSerializer<AbilityDex>.LoadFromFile("abilitydex.xml");

            if (abilitydex == null)
            {
                var abilities = await _pokemonRetriever.RetrieveAllAbilities().ConfigureAwait(false);

                abilitydex = new AbilityDex(abilities);

                GenericSerializer<AbilityDex>.SaveToFile(abilitydex, "abilitydex.xml");
            }

            return abilitydex;
        }
    }
}
