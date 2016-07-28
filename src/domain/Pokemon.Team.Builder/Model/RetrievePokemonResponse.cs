using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon.Team.Builder
{
    public class RetrievePokemonResponse
    {
        public int Count { get; set; }
        public object Previous { get; set; }
        public List<SimplePokemonData> Results { get; set; }
        public string Next { get; set; }        
    }
}
