using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon.Team.Builder.Model
{
    public class RetrievePokemonUsageResponse
    {
        public string status_code { get; set; }
        public List<RankingPokemonInfo> rankingPokemonInfo { get; set; }
        public string updateDate { get; set; }
        public string timezoneName { get; set; }
    }
}
