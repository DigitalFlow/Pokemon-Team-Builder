using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon.Team.Builder.Model
{
    public class RetrievePokemonUsageResponse
    {
		/// <summary>
		/// 0000 => OK, 4000 Otherwise
		/// </summary>
		/// <value>The status code.</value>
        public string Status_Code { get; set; }

		/// <summary>
		/// Moves that the current pokemon most often uses for fainting opponent pokemon
		/// </summary>
		/// <value>The ranking pokemon sufferer waza.</value>
        public List<RankingPokemonSuffererWaza> RankingPokemonSuffererWaza { get; set; }

		/// <summary>
		/// Pokemon that the current pokemon most often defeats
		/// </summary>
		/// <value>The ranking pokemon sufferer.</value>
        public List<RankingPokemonSufferer> RankingPokemonSufferer { get; set; }

		/// <summary>
		/// Pokemon that are most often used as team mates for the current pokemon
		/// </summary>
		/// <value>The ranking pokemon in.</value>
        public List<RankingPokemonIn> RankingPokemonIn { get; set; }

		/// <summary>
		/// Previous pokemon identifier in pokedex.
		/// </summary>
		/// <value>The before pokemon identifier.</value>
        public string BeforePokemonId { get; set; }

		/// <summary>
		/// Gets or sets the ranking pokemon trend.
		/// </summary>
		/// <value>The ranking pokemon trend.</value>
        public RankingPokemonTrend RankingPokemonTrend { get; set; }

		/// <summary>
		/// Information on the current pokemon
		/// </summary>
		/// <value>The ranking pokemon info.</value>
        public RankingPokemonInfo RankingPokemonInfo { get; set; }

		/// <summary>
		/// Pokemon that most often faint the current pokemon
		/// </summary>
		/// <value>The ranking pokemon down.</value>
        public List<RankingPokemonDown> RankingPokemonDown { get; set; }
        
		/// <summary>
		/// Ranking of opponent moves that do not faint current pokemon
		/// </summary>
		/// <value>The ranking pokemon down waza other.</value>
		public RankingPokemonDownWazaOther RankingPokemonDownWazaOther { get; set; }
        
		/// <summary>
		/// Next pokemon identifier in pokedex.
		/// </summary>
		/// <value>The next pokemon identifier.</value>
		public string NextPokemonId { get; set; }

		/// <summary>
		/// Moves that most often faint the current pokemon
		/// </summary>
		/// <value>The ranking pokemon down waza.</value>
        public List<RankingPokemonDownWaza> RankingPokemonDownWaza { get; set; }

		/// <summary>
		/// Name of the timezone.
		/// </summary>
		/// <value>The name of the timezone.</value>
        public string TimezoneName { get; set; }
    }
}
