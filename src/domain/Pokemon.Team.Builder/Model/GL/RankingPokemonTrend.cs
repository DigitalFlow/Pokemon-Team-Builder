using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon.Team.Builder.Model
{
    public class RankingPokemonTrend
    {
		/// <summary>
		/// Most often used abilities
		/// </summary>
		/// <value>The tokusei info.</value>
        public List<TokuseiInfo> TokuseiInfo { get; set; }

		/// <summary>
		/// Most often used natures
		/// </summary>
		/// <value>The seikaku info.</value>
        public List<SeikakuInfo> SeikakuInfo { get; set; }

		/// <summary>
		/// Most often used items
		/// </summary>
		/// <value>The item info.</value>
        public List<ItemInfo> ItemInfo { get; set; }

		/// <summary>
		/// Most often used moves
		/// </summary>
		/// <value>The waza info.</value>
        public List<WazaInfo> WazaInfo { get; set; }
    }
}
