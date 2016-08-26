using Pokemon.Team.Builder.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon.Team.Builder.Logic
{
    public class ItemdexManager
    {
        private IPokemonMetaDataRetriever _pokemonRetriever;

        public ItemdexManager(IPokemonMetaDataRetriever pokemonMetaDataRetriever)
        {
            _pokemonRetriever = pokemonMetaDataRetriever;
        }

        public async Task<Itemdex> GetItems()
        {
            var itemdex = await GenericSerializer<Itemdex>.LoadFromFile("itemdex.xml").ConfigureAwait(false);

            if (itemdex == null)
            {
                var items = await _pokemonRetriever.RetrieveAllItems().ConfigureAwait(false);

                itemdex = new Itemdex(items);

                await GenericSerializer<Itemdex>.SaveToFile(itemdex, "itemdex.xml").ConfigureAwait(false);
            }

            return itemdex;
        }
    }
}
