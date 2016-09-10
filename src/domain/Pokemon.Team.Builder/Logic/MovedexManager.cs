using System;
using System.Collections.Generic;
using System.IO;
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

        public async Task<Movedex> GetMoves(string filePath)
        {
            var movedex = await GenericSerializer<Movedex>.LoadFromFile (filePath).ConfigureAwait(false);

            if (movedex == null)
            {
                var items = await _pokemonRetriever.RetrieveAllMoves().ConfigureAwait(false);

                movedex = new Movedex(items);

                await GenericSerializer<Movedex>.SaveToFile(movedex, filePath).ConfigureAwait(false);
            }

            return movedex;
        }
    }
}
