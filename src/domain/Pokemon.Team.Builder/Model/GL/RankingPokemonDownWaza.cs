using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon.Team.Builder.Model
{
	public class RankingPokemonDownWaza : IEquatable<RankingPokemonDownWaza>, IRankable
    {
        public int Ranking { get; set; }
        public int TypeId { get; set; }
        public double UsageRate { get; set; }
        public string WazaName { get; set; }
        public int SequenceNumber { get; set; }

		public bool Equals (RankingPokemonDownWaza otherRankingDown) {
			if (otherRankingDown == null) {
				return false;
			}

			return TypeId == otherRankingDown.TypeId && WazaName == otherRankingDown.WazaName;
		}

		public override bool Equals (object obj)
		{
			var otherRankingDown = obj as RankingPokemonDownWaza;

			if (otherRankingDown == null) {
				return false;
			}

			return Equals(otherRankingDown);
		}

		public override int GetHashCode ()
		{
			return $"{WazaName}-{TypeId}".GetHashCode();
		}
    }
}
