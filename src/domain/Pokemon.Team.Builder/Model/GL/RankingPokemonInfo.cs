using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon.Team.Builder.Model
{
	public class RankingPokemonInfo : IRankable, IPokemonIdentifiable
    {
        public int MonsNo { get; set; }
        public string FormNo { get; set; }
        public string PokemonId { get; set; }
        public int Ranking { get; set; }
        public string TypeName1 { get; set; }
        public string TypeName2 { get; set; }
        public string Weight { get; set; }
        public int TypeId1 { get; set; }
        public int? TypeId2 { get; set; }
        public object FormName { get; set; }
        public string Name { get; set; }
        public int SequenceNumber { get; set; }
        public string Height { get; set; }

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

        public override bool Equals (object obj)
		{
			var otherRanking = obj as RankingPokemonInfo;

			if (otherRanking == null) {
				return false;
			}

			return MonsNo == otherRanking.MonsNo && FormNo == otherRanking.FormNo;
		}

		public override int GetHashCode ()
		{
			return base.GetHashCode ();
		}

		public override string ToString ()
		{
			return $"{MonsNo} - {Name}";
		}
    }
}
