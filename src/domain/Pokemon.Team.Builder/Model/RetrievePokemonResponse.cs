using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon.Team.Builder
{
    public class RetrievePokemonResponse
    {
        public int count { get; set; }
        public object previous { get; set; }
        public List<SimplePokemonData> results { get; set; }
        public string next { get; set; }        
    }
}
