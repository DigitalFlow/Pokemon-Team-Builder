using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon.Team.Builder.Model
{
    public class RetrievePokemonUsageResponse
    {
        public string Status_Code { get; set; }
        public List<RankingPokemonSuffererWaza> RankingPokemonSuffererWaza { get; set; }
        public List<RankingPokemonSufferer> RankingPokemonSufferer { get; set; }
        public List<RankingPokemonIn> RankingPokemonIn { get; set; }
        public string BeforePokemonId { get; set; }
        public RankingPokemonTrend RankingPokemonTrend { get; set; }
        public RankingPokemonInfo RankingPokemonInfo { get; set; }
        public List<RankingPokemonDown> RankingPokemonDown { get; set; }
        public RankingPokemonDownWazaOther RankingPokemonDownWazaOther { get; set; }
        public string NextPokemonId { get; set; }
        public List<RankingPokemonDownWaza> RankingPokemonDownWaza { get; set; }
        public string TimezoneName { get; set; }
    }
}
