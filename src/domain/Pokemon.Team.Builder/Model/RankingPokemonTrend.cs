using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon.Team.Builder.Model
{
    public class RankingPokemonTrend
    {
        public List<TokuseiInfo> tokuseiInfo { get; set; }
        public List<SeikakuInfo> seikakuInfo { get; set; }
        public List<ItemInfo> itemInfo { get; set; }
        public List<WazaInfo> wazaInfo { get; set; }
    }
}
