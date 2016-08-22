using Pokemon.Team.Builder.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon.Team.Builder.Model
{
	public class RankingPokemonIn : IEquatable<ITeamMate>, IRankable, ITeamMate
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
        public string FormName { get; set; }
        public string Name { get; set; }
        public int SequenceNumber { get; set; }

        public PokemonIdentifier Identifier
        {
            get
            {
                return new PokemonIdentifier(MonsNo, FormNo);
            }

            set
            {
                MonsNo = value.MonsNo;
                FormNo = value.FormNo;
            }
        }

        public bool Equals (ITeamMate otherRankingIn) {
			if (otherRankingIn == null) {
				return false;
			}

			return Identifier == otherRankingIn.Identifier;
		}

		public static implicit operator PokemonIdentifier (RankingPokemonIn rankingPokemon) {
			return new PokemonIdentifier (rankingPokemon.MonsNo, rankingPokemon.FormNo);
		}	

		public override bool Equals (object obj)
		{
			var otherRankingIn = obj as RankingPokemonIn;

			if (otherRankingIn == null) {
				return false;
			}

			return Equals(otherRankingIn);
		}

		public override int GetHashCode ()
		{
			return $"{MonsNo}-{FormNo}".GetHashCode();
		}
    }
}
