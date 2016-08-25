using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon.Team.Builder.Logic
{
    public class MovedexManager
    {
        private IPokemonMetaDataRetriever _pokemonRetriever;

        public MovedexManager(IPokemonMetaDataRetriever pokemonMetaDataRetriever)
        {
            _pokemonRetriever = pokemonMetaDataRetriever;
        }

        public async Task<Movedex> GetMoves()
        {
            var movedex = GenericSerializer<Movedex>.LoadFromFile ("movedex.xml");

            if (movedex == null)
            {
                var items = await _pokemonRetriever.RetrieveAllMoves().ConfigureAwait(false);

                movedex = new Movedex(items);

                GenericSerializer<Movedex>.SaveToFile(movedex, "movedex.xml");
            }

            return movedex;
        }
    }
}
