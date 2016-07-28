using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon.Team.Builder.Model
{
    public class RankingPokemonTrend
    {
        public List<TokuseiInfo> TokuseiInfo { get; set; }
        public List<SeikakuInfo> SeikakuInfo { get; set; }
        public List<ItemInfo> ItemInfo { get; set; }
        public List<WazaInfo> WazaInfo { get; set; }
    }
}
