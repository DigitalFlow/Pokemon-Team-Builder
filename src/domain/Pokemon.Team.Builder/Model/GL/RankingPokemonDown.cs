using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon.Team.Builder.Model
{
	public class RankingPokemonDown : IEquatable<RankingPokemonDown>, IRankable
    {
        public int MonsNo { get; set; }
        public string FormNo { get; set; }
        public string PokemonId { get; set; }
        public int Ranking { get; set; }
        public int CountBattleByForm { get; set; }
        public int BattlingChangeFlg { get; set; }
        public string TypeName1 { get; set; }
        public string TypeName2 { get; set; }
        public int TypeId1 { get; set; }
        public int? TypeId2 { get; set; }
        public object FormName { get; set; }
        public string Name { get; set; }
        public int SequenceNumber { get; set; }

		public bool Equals (RankingPokemonDown otherRankingDown) {
			if (otherRankingDown == null) {
				return false;
			}

			return MonsNo == otherRankingDown.MonsNo && FormNo == otherRankingDown.FormNo;
		}

		public static implicit operator PokemonIdentifier (RankingPokemonDown rankingPokemon) {
			return new PokemonIdentifier (rankingPokemon.MonsNo, rankingPokemon.FormNo);
		}	

		public override bool Equals (object obj)
		{
			var otherRankingDown = obj as RankingPokemonDown;

			if (otherRankingDown == null) {
				return false;
			}

			return Equals(otherRankingDown);
		}

		public override int GetHashCode ()
		{
			return $"{MonsNo}-{FormNo}".GetHashCode();
		}
    }
}
